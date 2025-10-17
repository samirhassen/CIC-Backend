import { Component, ViewChild, ElementRef, CUSTOM_ELEMENTS_SCHEMA, OnInit, HostListener, Inject, PLATFORM_ID } from '@angular/core';
import { homeService } from './home_service';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { ConstantRoute } from '../../ConstantsRoutes';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, MatProgressSpinnerModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})

export class HomeComponent implements OnInit {
  @ViewChild('embedContainer', { static: false }) embedContainer!: ElementRef;
  screenWidth: number = 0;
  screenHeight: number = 0;
  embedConfig: any;
  isBrowser = false;
  loading: boolean = true;
  private readonly LOCAL_STORAGE_KEY = 'powerBIReport';
  securityToken :string;
  constructor(
    private router: Router,
    private snackBar: MatSnackBar,
    private homeService: homeService,
    private route: ActivatedRoute,
    private reportService: homeService,
    @Inject(PLATFORM_ID) private platformId: Object) {
    this.isBrowser = isPlatformBrowser(this.platformId);
  }

  ngOnInit(): void {

    if (this.isBrowser) {
      this.route.queryParams.subscribe((params: { [x: string]: any }) => {
        const token = params['token'];
        if (token) {
          this.securityToken = token;
          this.homeService.getAuthenticateToken(token).subscribe({
            next: (res: any) => {
              if (res.result.status === 'error') {
                this.loading = false;
                this.snackBar.open(ConstantRoute.tokenErrorMassege, '', {
                  duration: 3000,
                  horizontalPosition: 'right',
                  verticalPosition: 'top',
                  panelClass: ['snackbar-error']
                });

                setTimeout(() => {
                  window.location.href=ConstantRoute.RedirectUrl;                 
                }, 2000);
              }
              else {
                
                this.updateScreenSize();
                this.loadPowerBIReport();
                this.loading = false;
              }
            },
            error: (err: any) => console.error('Error:', err)
          });
        }
        else{
          window.location.href=ConstantRoute.RedirectUrl;

        }
      });
    }
  }

  private loadPowerBIReport() {
    if (!this.isBrowser) {
      console.warn('Power BI is only available in the browser.');
      return;
    }

    const cachedReport = this.getCachedReport();
    const nowUTC = new Date();
    const expirationUTC = new Date(cachedReport?.expiration);
    this.getPowerBIReport(); // Fallback to API call
    if (cachedReport && expirationUTC?.getTime() > nowUTC?.getTime()) {
      this.embedPowerBIReport(cachedReport);
    } else {
      this.getPowerBIReport(); // Fallback to API call
    }
  }


  //Read Report data from Localstroge
  private getCachedReport(): any | null {
    const stored = localStorage.getItem(this.LOCAL_STORAGE_KEY);
    return stored ? JSON.parse(stored) : null;
  }

  //Bind Report
  private getPowerBIReport(): void {
    this.reportService.getReport(this.securityToken).subscribe({
      next: async (res: any) => {
        if (res) {
          localStorage.setItem(this.LOCAL_STORAGE_KEY, JSON.stringify(res));
          this.embedPowerBIReport(res); // Use it
        } else {
          console.error('Invalid report response');
          this.loading = false;
        }
      },
      error: (err: any) => {
        console.error('Error fetching report:', err);
        this.loading = false;
      }
    });
  }

  //Bind Report
  private async embedPowerBIReport(report: any): Promise<void> {
    const pbiModule = await import('powerbi-client');
    const pbi = pbiModule.default ?? pbiModule;

    const embedConfiguration: any = {
      type: 'report',
      id: report.reportID,
      embedUrl: report.embedUrl,
      accessToken: report.token,
      tokenType: pbi?.models?.TokenType?.Embed,
      settings: {
        panes: {
          filters: { visible: true },
          pageNavigation: { visible: true }
        }
      }
    };

    if (pbi.service) {
      const powerbi = new pbi.service.Service(
        pbi.factories.hpmFactory,
        pbi.factories.wpmpFactory,
        pbi.factories.routerFactory
      );

      powerbi.embed(this.embedContainer.nativeElement, embedConfiguration);
    }

    this.loading = false;
  }

  @HostListener('window:resize')
  onResize() {
    this.updateScreenSize();
  }
  private updateScreenSize(): void {
    this.screenWidth = window.innerWidth;
    this.screenHeight = window.innerHeight - 70;
  }

  // getPowerBIReport() {
  //   if (!this.isBrowser) {
  //     console.warn('Power BI is only available in the browser.');
  //     return;
  //   }

  //   this.reportService.getReport().subscribe({
  //     next: async (res: any) => {
  //       if (res) {
  //         const pbiModule = await import('powerbi-client');
  //         const pbi = pbiModule.default ?? pbiModule;

  //         const embedConfiguration: any = {
  //           type: 'report',
  //           id: res.reportID,
  //           embedUrl: res.embedUrl,
  //           accessToken: res.token,
  //           tokenType: pbi?.models?.TokenType?.Embed,
  //           settings: {
  //             panes: {
  //               filters: { visible: true },
  //               pageNavigation: { visible: true }
  //             }
  //           }
  //         };

  //         if (pbi.service) {
  //           const powerbi = new pbi.service.Service(
  //             pbi.factories.hpmFactory,
  //             pbi.factories.wpmpFactory,
  //             pbi.factories.routerFactory
  //           );

  //           powerbi.embed(this.embedContainer.nativeElement, embedConfiguration);
  //           this.loading = false;
  //         }

  //       } else {
  //         this.loading = false;
  //         console.error('Invalid response:', res);
  //       }
  //     },
  //     error: (err: any) => {
  //       this.loading = false;
  //       console.error('Error fetching report:', err);
  //     }
  //   });
  // }
}