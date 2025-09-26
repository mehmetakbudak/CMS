import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Router } from '@angular/router';
import { BehaviorSubject, tap } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {
  key = 'currentUser';
  private isAuthenticate = new BehaviorSubject<boolean>(false);
  isAuthenticate$ = this.isAuthenticate.asObservable();

  constructor(private http: HttpClient, private router: Router) {}

  login(data: any) {
    return this.http.post<any>(`${environment.apiUrl}Auth/Login`, data);
  }

  logout() {
    localStorage.removeItem(this.key);
    localStorage.removeItem('translations');
    this.isAuthenticate.next(false);
    this.router.navigate(['/login'], {
      queryParams: {
        returnUrl: this.router.routerState.snapshot.url,
      },
    });
  }

  getUserInfo() {
    var str = localStorage.getItem(this.key);
    if (str) return JSON.parse(str);
    else return null;
  }

  isAuthenticated(): boolean {
    const user = this.getUserInfo();
    if (user) {
      if (new Date(user.refreshTokenExpireDate) < new Date()) {
        this.isAuthenticate.next(false);
        return false;
      }
      return true;
    } else {
      return false;
    }
  }

  setUserInfo(data: string): void {
    localStorage.setItem(this.key, JSON.stringify(data));
  }

  refreshToken() {
    const user: any = this.getUserInfo();
    return this.http
      .post(`${environment.apiUrl}Auth/CreateTokenByRefreshToken`, {
        refreshToken: user.refreshToken,
      })
      .pipe(
        tap((res: any) => {
          this.setUserInfo(res);
        })
      );
  }

  setIsAuthenticate(value: boolean) {
    return this.isAuthenticate.next(value);
  }
}
