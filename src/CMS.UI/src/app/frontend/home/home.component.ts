import { environment } from './../../../environments/environment';
import { Component, model, OnInit } from '@angular/core';
import { AppService } from '../../services/app.service';
import { GalleriaModule } from 'primeng/galleria';
import { CommonModule } from '@angular/common';
import { CarouselModule } from 'primeng/carousel';
import { PanelModule } from 'primeng/panel';
import { Router, RouterLink } from '@angular/router';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
  imports: [
    CommonModule,
    GalleriaModule,
    CarouselModule,
    PanelModule,
    RouterLink,
    ButtonModule,
  ],
})
export class HomeComponent implements OnInit {
  environment = environment;
  responsiveOptions: any[] | undefined;
  homepageSliders: any = [];
  blogs: any = [];
  jobs: any = [];
  homeSliderImgHeight = 500;

  constructor(private appService: AppService, private router: Router) {}

  ngOnInit() {
    this.getHomepageSliders();
    this.getJobs();
    this.getBlogs();
    if (window.screen.width < 768) {
      this.homeSliderImgHeight = 300;
    } else {
      this.homeSliderImgHeight = 500;
    }
    this.responsiveOptions = [
      {
        breakpoint: '1400px',
        numVisible: 2,
        numScroll: 1,
      },
      {
        breakpoint: '1199px',
        numVisible: 3,
        numScroll: 1,
      },
      {
        breakpoint: '767px',
        numVisible: 2,
        numScroll: 1,
      },
      {
        breakpoint: '575px',
        numVisible: 1,
        numScroll: 1,
      },
    ];
  }

  getHomepageSliders() {
    this.appService.get('HomepageSlider/GetAllActive').subscribe((res: any) => {
      this.homepageSliders = res;
    });
  }

  getJobs() {
    this.appService.get('Job/GetAllActive').subscribe((res: any) => {
      this.jobs = res;
    });
  }

  getBlogs() {
    this.appService.get('Blog/GetAllActive').subscribe((res: any) => {
      this.blogs = res;
    });
  }

  onSliderClick(item: any) {
    if (item.url) {
      this.router.navigateByUrl(item.url);
    }
  }
}
