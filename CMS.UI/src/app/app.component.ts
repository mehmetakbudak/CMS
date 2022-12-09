import { Component } from '@angular/core';
import { LoginResponseModel } from './services/login-response.model';
import { TokenStorageService } from './services/token-storage.service';
import { UserType } from './storage/enums/user-type';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'app';
  private roles: string[] = [];
  isLoggedIn = false;
  showAdminBoard = false;
  showModeratorBoard = false;
  user?: LoginResponseModel;  
  UserType = UserType;

  constructor(private tokenStorageService: TokenStorageService) {}

  ngOnInit(): void {
    this.isLoggedIn = !!this.tokenStorageService.getTokenInfo();

    if (this.isLoggedIn) {
      this.user = this.tokenStorageService.getTokenInfo();
    }
  }

  logout(): void {
    this.tokenStorageService.signOut();
    window.location.href= "/";
  }
}
