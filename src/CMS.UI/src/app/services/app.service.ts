import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root',
})
export class AppService {
  constructor(
    private http: HttpClient,
    private translateService: TranslateService
  ) {}

  getTranslate(key: string) {
    return this.translateService.instant(key);
  }

   getLang(): string {
    return this.translateService.currentLang;
  }

  onLangChange() {
    return this.translateService.onLangChange.asObservable();
  }

  get(url: string) {
    return this.http.get(environment.apiUrl + url);
  }

  post(url: string, data: any) {
    return this.http.post(environment.apiUrl + url, data);
  }

  put(url: string, data: any) {
    return this.http.put(environment.apiUrl + url, data);
  }

  delete(url: string, id: any) {
    return this.http.delete(environment.apiUrl + url + '/' + id);
  }
}
