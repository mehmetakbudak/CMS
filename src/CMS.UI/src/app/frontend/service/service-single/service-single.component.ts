import { Component, OnInit } from '@angular/core';
import { SharedModule } from '../../../shared.module';
import { AppService } from '../../../services/app.service';
import { environment } from '../../../../environments/environment';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-service-single',
  imports: [SharedModule],
  templateUrl: './service-single.component.html',
  styleUrl: './service-single.component.css',
})
export class ServiceSingleComponent implements OnInit {
  service: any = {};
  services: any = [];
  environment = environment;

  constructor(private route: ActivatedRoute, private apiService: AppService) {}

  ngOnInit(): void {
    this.getList();
    this.route.params.subscribe((params) => {
      var url: string = params['url'];
      if (url) {
        this.getByUrl(url);
      }
    });
  }

  getList() {
    this.apiService.get('Service/Get').subscribe((res: any) => {
      this.services = res.data;
    });
  }

  getByUrl(url: string) {
    this.apiService.get(`Service/GetByUrl/${url}`).subscribe((res: any) => {
      this.service = res;
    });
  }
}
