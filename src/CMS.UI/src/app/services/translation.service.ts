import { TranslateService } from '@ngx-translate/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class TranslationService {
  constructor(
    private http: HttpClient,
    private translateService: TranslateService
  ) {}
  private langSource = new BehaviorSubject<string>(
    localStorage.getItem('lang') || 'tr-TR'
  );
  currentLang = this.langSource.asObservable();

  init() {
    const localData = localStorage.getItem('translations');
    if (localData) {
      this.load(JSON.parse(localData));
    } else {
      this.http
        .get<any[]>(`${environment.apiUrl}Language/GetDictionary`)
        .subscribe((data) => {
          localStorage.setItem('translations', JSON.stringify(data));
          this.load(data);
        });
    }
  }

  private load(data: any[]) {
    const currentLang = localStorage.getItem('lang') || 'tr-TR';
    const langData = data.find((x) => x.code === currentLang);
    if (langData) {
      this.translateService.setTranslation(currentLang, langData.items, true);
      this.translateService.use(currentLang);
    }
  }

  changeLang(code: string) {
    const data = JSON.parse(localStorage.getItem('translations') || '[]');
    const langData = data.find((x: any) => x.code === code);
    if (langData) {
      this.translateService.setTranslation(code, langData.items, true);
      this.translateService.use(code);
      localStorage.setItem('lang', code);
      this.langSource.next(code);
    }
  }
}
