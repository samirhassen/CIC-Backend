import { CommonModule } from '@angular/common';
import { Component, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatToolbarModule } from '@angular/material/toolbar';
import { Router } from '@angular/router';
import { msalInitialized, msalInstance } from './msal-config';


@Component({
  selector: 'app-login',
  imports: [MatTableModule, MatSortModule, MatIconModule, MatButtonModule, MatToolbarModule,
    MatButtonModule,
    MatIconModule, CommonModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  constructor(private router: Router) { }

  signInWithMicrosoft(): void {
    this.router.navigate(['/home']);
  }

  signInWithUsername(): void {
    this.router.navigate(['/authentication']);
  }

  async login() {
    await msalInitialized; // 👈 Ensure MSAL is initialized
    const result = await msalInstance.loginPopup({
      scopes: ['api://fc8572b1-78e6-49e8-8fa9-a78fdbe71f7b/access_as_user']
    });

    msalInstance.setActiveAccount(result.account);
    this.router.navigate(['/home']);
    console.log('Login successful:', result.account?.username);
  }
}


