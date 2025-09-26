import { environment } from '../../../environments/environment';
import { SharedModule } from '../../shared.module';
import { AppService } from './../../services/app.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-testimonial',
  imports: [SharedModule],
  templateUrl: './testimonial.component.html',
  styleUrl: './testimonial.component.css',
})
export class FE_TestimonialComponent implements OnInit {
  list: any = [];
  total = 0;
  first: number = 0;
  rows: number = 4;

  environment = environment;

  constructor(private appService: AppService) {}

  ngOnInit() {
    this.getList();
  }

  getList() {
    this.appService
      .get(`Testimonial/Get?$skip=${this.first}&$top=${this.rows}&$count=true`)
      .subscribe((res: any) => {
        this.list = res.data;
        this.total = res.total;
      });
  }

  onPageChange(e: any) {
    this.first = e.first;
    this.rows = e.rows;
    this.getList();
  }
}
