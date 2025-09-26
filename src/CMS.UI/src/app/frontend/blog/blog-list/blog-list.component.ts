import { environment } from './../../../../environments/environment';
import { AppService } from './../../../services/app.service';
import { Component, OnInit } from '@angular/core';
import { SharedModule } from '../../../shared.module';

@Component({
    selector: 'app-blog-list',
    imports: [SharedModule],
    templateUrl: './blog-list.component.html',
    styleUrl: './blog-list.component.css'
})
export class FE_BlogListComponent implements OnInit {
  blogs: any = [];
  mostReads: any = [];
  blogCategories: any = [];
  total: number = 0;
  skip: number = 0;
  pageSize: number = 4;
  environment = environment;

  constructor(private appService: AppService) {}

  ngOnInit(): void {
    this.getBlogs();
    this.getMostRead();
    this.getBlogCategories();
  }

  getBlogs() {
    this.appService
      .post('Blog/List', {
        skip: this.skip,
        pageSize: this.pageSize,
      })
      .subscribe((res: any) => {
        this.blogs = res.list;
        this.total = res.total;
      });
  }

  getMostRead() {
    this.appService.get('Blog/MostRead').subscribe((res: any) => {
      this.mostReads = res;
    });
  }

  getBlogCategories() {
    this.appService.get('BlogCategory/GetAllWithCount').subscribe((res: any) => {
      this.blogCategories = res;
    });
  }

  onPageChange(event: any) {
    this.skip = event.first;
    this.pageSize = event.rows;
    this.getBlogs();
  }
}
