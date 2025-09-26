import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { AppService } from '../../services/app.service';
import { TreeNode } from 'primeng/api';
import { SharedModule } from '../../shared.module';
import { TranslationService } from '../../services/translation.service';

@Component({
  selector: 'app-admin-layout',
  templateUrl: './admin-layout.component.html',
  styleUrls: ['./admin-layout.component.css'],
  imports: [SharedModule],
})
export class AdminLayoutComponent implements OnInit {
  contentClass = '';
  items: TreeNode[] = [];
  userMenu: any = [];
  languages: any = [];
  currentUser: any;
  fullName: any;
  visibleDrawer = false;
  selectedLanguageCode: any;

  constructor(
    private authService: AuthService,
    private appService: AppService,
    private router: Router,
    private translationService: TranslationService
  ) {}

  ngOnInit() {
    this.currentUser = this.authService.getUserInfo();
    this.fullName = this.currentUser.fullName;

    this.appService.get(`Menu/GetAdminMenu`).subscribe({
      next: (res: any) => {
        this.items = res;
      },
      error: (err) => {
        console.error(err);
      },
    });
    this.appService.get(`Language/Get`).subscribe((res: any) => {
      this.languages = res.data;
      var selectedLang = localStorage.getItem('lang');
      if (selectedLang) {
        this.selectedLanguageCode = selectedLang;
      } else {
        this.selectedLanguageCode = this.languages.find(
          (x: any) => x.isDefault === true
        )?.code;
        localStorage.setItem('lang', this.selectedLanguageCode);
      }
      this.translationService.init();
    });

    this.getUserMenuItems();

    this.appService.onLangChange().subscribe(() => {
      this.getUserMenuItems();
    });
  }

  getUserMenuItems() {
    this.userMenu = [
      { label: this.appService.getTranslate('Homepage'), routerLink: '/' },
      {
        label: this.appService.getTranslate('Logout'),
        command: () => {
          this.authService.logout();
          this.router.navigateByUrl('/login');
        },
      },
    ];
  }

  menuShow() {
    this.visibleDrawer = true;
  }

  selectUserMenu(e: any) {
    if (e.itemData.isParam) {
      this.router.navigateByUrl(e.itemData.url);
    } else {
      this.router.navigate([e.itemData.url]);
    }
  }

  changeLanguage(e: any) {
    this.translationService.changeLang(e.value);
  }
}
