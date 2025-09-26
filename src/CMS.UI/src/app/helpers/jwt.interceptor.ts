import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { HttpRequest, HttpHandlerFn, HttpEvent } from '@angular/common/http';
import {
  Observable,
  catchError,
  switchMap,
  throwError,
  BehaviorSubject,
  filter,
  take,
} from 'rxjs';
import { AuthService } from '../services/auth.service';
import { AppService } from '../services/app.service';

let isRefreshing = false;
const refreshTokenSubject = new BehaviorSubject<string | null>(null);

export const authInterceptor: HttpInterceptorFn = (
  req: HttpRequest<any>,
  next: HttpHandlerFn
): Observable<HttpEvent<any>> => {
  const authService = inject(AuthService);
  const appService = inject(AppService);
  const user = authService.getUserInfo();
  const lang: any = appService.getLang();

  if (user) {
    if (lang) {
      req = req.clone({
        setHeaders: {
          Authorization: `Bearer ${user.accessToken}`,
          Language: lang,
        },
      });
    } else {
      req = req.clone({
        setHeaders: {
          Authorization: `Bearer ${user.accessToken}`,
        },
      });
    }
  } else {
    if (lang) {
      req = req.clone({
        setHeaders: {
          Language: lang,
        },
      });
    }
  }

  return next(req).pipe(
    catchError((err) => {
      if (err.status === 401) {
        return handle401Error(req, next, authService);
      }
      return throwError(() => err);
    })
  );
};

function handle401Error(
  req: HttpRequest<any>,
  next: HttpHandlerFn,
  authService: AuthService
): Observable<HttpEvent<any>> {
  if (!isRefreshing) {
    isRefreshing = true;
    refreshTokenSubject.next(null);

    return authService.refreshToken().pipe(
      switchMap((res: any) => {
        isRefreshing = false;
        refreshTokenSubject.next(res.accessToken);
        return next(
          req.clone({
            setHeaders: { Authorization: `Bearer ${res.accessToken}` },
          })
        );
      }),
      catchError((err) => {
        isRefreshing = false;
        authService.logout();
        return throwError(() => err);
      })
    );
  } else {
    return refreshTokenSubject.pipe(
      filter((token) => token !== null),
      take(1),
      switchMap((token: any) => {
        console.log(token);
        return next(
          req.clone({ setHeaders: { Authorization: `Bearer ${token}` } })
        );
      })
    );
  }
}
