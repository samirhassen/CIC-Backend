import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ConstantRoute } from '../ConstantsRoutes';

@Injectable({ providedIn: 'root' })
export class AuthenticationServices {
  private apiURL = ConstantRoute.API_URL;
  private getAuthURL = this.apiURL +'/authenticateUser';

  constructor(private http: HttpClient) {}

  getAuthentication(username: string, password: string): Observable<any> {
    const body = { username, password }; // send as JSON body
    return this.http.post<any>(this.getAuthURL, body);
  }
}