import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HttpParams } from '@angular/common/http';
import { ConstantRoute } from '../../ConstantsRoutes';

@Injectable({ providedIn: 'root' })
export class headerService {
  private apiURL = ConstantRoute.API_URL;
  private logoutURL = this.apiURL + '/deleteUserSession';

  constructor(private http: HttpClient) {}


  getLogOut(): Observable<any> {
    const token = localStorage.getItem('token') ?? '';
    const params = new HttpParams().set('token', token);
    return this.http.get<any>(this.logoutURL, { params });
  }

}