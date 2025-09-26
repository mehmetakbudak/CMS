import { Component, OnInit } from '@angular/core';
import { NavMenuComponent } from '../nav-menu/nav-menu.component';
import { SharedModule } from '../../shared.module';

@Component({
  selector: 'app-frontend-layout',
  templateUrl: './frontend-layout.component.html',
  styleUrls: ['./frontend-layout.component.css'],
  imports: [SharedModule, NavMenuComponent],
})
export class FrontendLayoutComponent implements OnInit {
  items: any;

  constructor() {}

  ngOnInit() {

  }
}
