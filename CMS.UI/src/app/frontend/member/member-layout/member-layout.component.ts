import { Component, OnInit } from '@angular/core';
import { LoginResponseModel } from 'src/app/services/login-response.model';
import { TokenStorageService } from 'src/app/services/token-storage.service';

@Component({
  selector: 'app-member-layout',
  templateUrl: './member-layout.component.html',
  styleUrls: ['./member-layout.component.css'],
})
export class MemberLayoutComponent implements OnInit {
  user!: LoginResponseModel;

  constructor(private tokenStorageService: TokenStorageService) {}

  ngOnInit() {
    this.user = this.tokenStorageService.getTokenInfo();
  }

  logout() {
    this.tokenStorageService.signOut();
    window.location.href = '/';
  }
}
