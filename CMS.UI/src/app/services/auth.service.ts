import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.prod';
import { RegisterModel } from '../storage/models/register.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private http: HttpClient) { }

  login(model: any): Observable<any> {
    return this.http.post(`${environment.ApiUrl}account/login`, model);
  }

  register(model: RegisterModel): Observable<any> {
    return this.http.post(`${environment.ApiUrl}account/register`, model);
  }
}
