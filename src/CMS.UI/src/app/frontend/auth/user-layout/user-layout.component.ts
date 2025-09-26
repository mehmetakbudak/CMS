import { AuthService } from './../../../services/auth.service';
import { Component, OnInit } from '@angular/core';
import { PanelModule } from 'primeng/panel';
import { RouterLink, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-user-layout',
  imports: [PanelModule, RouterLink, RouterOutlet],
  templateUrl: './user-layout.component.html',
  styleUrl: './user-layout.component.css',
})
export class UserLayoutComponent implements OnInit {
  constructor(private authService: AuthService) {}

  ngOnInit() {}

  logout() {
    this.authService.logout();
  }
}
