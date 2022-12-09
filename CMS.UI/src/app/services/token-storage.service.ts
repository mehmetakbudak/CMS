import { Injectable } from '@angular/core';
import { LoginResponseModel } from './login-response.model';
import { BehaviorSubject, Observable } from 'rxjs';

const TOKEN_KEY = 'auth-token';

@Injectable({
  providedIn: 'root',
})
export class TokenStorageService {
  private currentUserSubject!: BehaviorSubject<LoginResponseModel>;

  constructor() {
    var sessionStorage: string | null =
      window.sessionStorage.getItem(TOKEN_KEY);
    if (sessionStorage) {
      this.currentUserSubject = new BehaviorSubject<LoginResponseModel>(
        JSON.parse(sessionStorage)
      );
    }
  }

  signOut(): void {
    window.sessionStorage.clear();
    this.currentUserSubject.next(new LoginResponseModel());
  }

  public saveTokenInfo(token: string): void {
    window.sessionStorage.removeItem(TOKEN_KEY);
    window.sessionStorage.setItem(TOKEN_KEY, token);
  }

  public getTokenInfo(): LoginResponseModel {
    return this.currentUserSubject?.value;
  }
}
