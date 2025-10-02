import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HttpParams } from '@angular/common/http';
import { ConstantRoute } from '../../ConstantsRoutes';

@Injectable({ providedIn: 'root' })
export class homeService {
  //private apiURL = 'https://localhost:7181';

  private apiURL =ConstantRoute.API_URL;
  private getReportURL = this.apiURL + '/api/PowerBIReportEmbed/getReport';
  private getAuthenticateTokenURL = this.apiURL + '/authenticateToken';

  constructor(private http: HttpClient) {}

  // getReport(): Observable<any> {
  //   return this.http.get<any>(this.getReportURL);
  // }

  getReport(): Observable<any> {
    const token = localStorage.getItem('token') ?? '';
    const params = new HttpParams().set('token', token);
  
    return this.http.get<any>(this.getReportURL, { params });
  }

  getAuthenticateToken(token: string): Observable<any> {
    return this.http.get<any>(this.getAuthenticateTokenURL, {
      params: { token }
    });
  }
}
