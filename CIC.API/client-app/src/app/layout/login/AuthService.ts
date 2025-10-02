import { Injectable } from '@angular/core';
import { MsalService } from '@azure/msal-angular';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private loggedIn = new BehaviorSubject<boolean>(false);

  constructor(private msalService: MsalService) {
    this.checkAccount();
  }

  checkAccount() {
    const accounts = this.msalService.instance.getAllAccounts();
    this.loggedIn.next(accounts.length > 0);
  }

  login() {
    
    this.msalService.loginRedirect();
  }

  logout() {
    this.msalService.logout();
  }

  get isLoggedIn() {
    return this.loggedIn.asObservable();
  }

  get accessToken() {
    return this.msalService.acquireTokenSilent({
      scopes: ['api://fc8572b1-78e6-49e8-8fa9-a78fdbe71f7b']
    });
  }
}