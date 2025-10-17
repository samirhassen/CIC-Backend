import { CommonModule } from '@angular/common';
import { Component, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { AuthenticationServices } from './AuthenticationServices';
import { Router } from '@angular/router';
import { ConstantRoute } from '../ConstantsRoutes';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-authentication',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatInputModule,
    MatFormFieldModule,
    MatCardModule,
    MatProgressSpinnerModule
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './authentication.component.html',
  styleUrls: ['./authentication.component.scss']
})
export class AuthenticationComponent {

  constructor(private snackBar: MatSnackBar, private authService: AuthenticationServices, private router: Router) { 

    
  }
  username: string = '';
  password: string = '';
  loading: boolean = false;


  login(): void {
    if (this.username && this.password) {
      this.loading = true;
      this.authService.getAuthentication(this.username, this.password).subscribe({
        next: (res: any) => {
          this.loading = false;
          if (res && res.pa_token) {
            localStorage.setItem('token', res.pa_token);
            this.router.navigate(['/home'], { queryParams: { token: res.pa_token } });
          }
          else {
            this.loading = false;
            this.snackBar.open(ConstantRoute.authenticateUser, '', {
              duration: 5000,
              horizontalPosition: 'right',
              verticalPosition: 'bottom',
              panelClass: ['snackbar-error']
            });
          }
        },
      });
    }
  }
}
