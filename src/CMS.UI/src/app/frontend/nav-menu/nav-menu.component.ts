import { Component, OnInit, Input, computed, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { AuthService } from '../../services/auth.service';
import { UserType } from '../../models/enums';
import { Subscription } from 'rxjs';
import { MenubarModule } from 'primeng/menubar';
import { DrawerModule } from 'primeng/drawer';
import { ButtonModule } from 'primeng/button';
import { CommonModule } from '@angular/common';
import { MenuModule } from 'primeng/menu';
import { TreeModule } from 'primeng/tree';
import { AppService } from '../../services/app.service';
import { Select } from 'primeng/select';
import { TranslationService } from '../../services/translation.service';
import { SharedModule } from '../../shared.module';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css'],
  imports: [SharedModule],
})
export class NavMenuComponent implements OnInit {
  items: any = [];
  mobileMenuItems: any = [];
  userMenuItems: MenuItem[] = [];
  languages: any = [];
  private authService = inject(AuthService);
  private router = inject(Router);
  isAuthenticateSubscription: Subscription;
  visibleDrawer = false;
  visibleMenu = false;
  selectedLanguageCode: any;

  constructor(
    private appService: AppService,
    private translationService: TranslationService
  ) {
    this.isAuthenticateSubscription =
      this.authService.isAuthenticate$.subscribe((res: boolean) => {
        this.getUserMenuItems(res);
      });
    this.visibleMenu = window.screen.width >= 576 ? true : false;
    if (window.screen.width <= 576) {
      this.appService.get('Menu/GetFrontendTreeMenu').subscribe((res: any) => {
        this.mobileMenuItems = res;
      });
    }
  }

  ngOnInit() {
    this.userMenuItems = [];
    const isAuthenticated = this.authService.isAuthenticated();
    this.getUserMenuItems(isAuthenticated);
    this.getMenus();
    this.getLanguages();
    this.appService.onLangChange().subscribe(() => {
      this.getUserMenuItems(isAuthenticated);
    });
  }

  getLanguages() {
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
  }

  changeLanguage(e: any) {
    this.translationService.changeLang(e.value);
  }

  getMenus() {
    if (this.visibleMenu) {
      this.appService.get('Menu/GetFrontendMenu').subscribe((res: any) => {
        this.items = res;
      });
    }
  }

  getUserMenuItems(isAuthenticate: boolean) {
    this.userMenuItems = [];
    var user = this.authService.getUserInfo();
    if (isAuthenticate) {
      this.userMenuItems.push({ icon: 'pi pi-user', label: user.fullName });
      if (user.userType != UserType.Member) {
        this.userMenuItems.push({
          icon: 'pi pi-cog',
          label: this.appService.getTranslate('AdminPanel'),
          routerLink: 'admin',
        });
      }
      var items = [
        {
          icon: 'pi pi-address-book',
          label: this.appService.getTranslate('Profile'),
          routerLink: 'user/profile',
        },
        {
          icon: 'pi pi-sign-out',
          label: this.appService.getTranslate('Logout'),
          command: () => {
            this.authService.logout();
            this.router.navigateByUrl('/login');
          },
        },
      ];
      items.forEach((item) => {
        this.userMenuItems.push(item);
      });
    } else {
      this.userMenuItems = [
        { label: this.appService.getTranslate('Login'), routerLink: 'login' },
        { label: this.appService.getTranslate('Register'), routerLink: 'register' },
      ];
    }
  }

  menuItemSelect(e: any) {
    if (e.itemData.url) {
      this.router.navigateByUrl(e.itemData.url);
    }
  }

  ngOnDestroy(): void {
    if (this.isAuthenticateSubscription) {
      this.isAuthenticateSubscription.unsubscribe();
    }
  }

  showMenu() {
    this.visibleDrawer = true;
  }
}
