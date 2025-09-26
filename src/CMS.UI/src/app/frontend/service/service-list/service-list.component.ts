import { environment } from './../../../../environments/environment';
import { Component, OnInit } from '@angular/core';
import { SharedModule } from '../../../shared.module';
import { AppService } from '../../../services/app.service';

@Component({
  selector: 'app-service-list',
  imports: [SharedModule],
  templateUrl: './service-list.component.html',
  styleUrl: './service-list.component.css',
})
export class FE_ServiceListComponent implements OnInit {
  list: any = [];
  environment = environment;

  constructor(private apiService: AppService) {}

  ngOnInit(): void {
    this.getList();
  }

  getList() {
    this.apiService.get('Service/Get').subscribe((res: any) => {
      this.list = res.data;
    });
  }
}
