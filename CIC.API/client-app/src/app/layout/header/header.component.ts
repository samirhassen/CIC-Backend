import { Component } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { Router, RouterModule } from '@angular/router';
import { FlexLayoutModule } from '@angular/flex-layout';
import { headerService } from './header-service';

@Component({
  selector: 'app-header',
  imports: [MatToolbarModule, MatButtonModule, RouterModule, FlexLayoutModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
constructor(private router: Router,private hearderService: headerService) {}


logout(): void {

  this.hearderService.getLogOut().subscribe({
    next: (res: any) => {
      console.log('Logout success:', res);

      // Clear local storage/session storage
      localStorage.removeItem('token');
      sessionStorage.clear();

      // Redirect to login page
      this.router.navigate(['/']);
    },
    error: (err: any) => {
      console.error('Logout failed:', err);    
      localStorage.removeItem('token');
      sessionStorage.clear();
      this.router.navigate(['/']);
    }
  });
}

}
