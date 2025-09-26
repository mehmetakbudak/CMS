import { Component, OnInit } from '@angular/core';
import { SharedModule } from '../../../shared.module';
import { AppService } from '../../../services/app.service';
import { environment } from '../../../../environments/environment';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-blog-category',
  templateUrl: './blog-category.component.html',
  styleUrls: ['./blog-category.component.scss'],
  imports: [SharedModule],
})
export class FE_BlogCategoryComponent implements OnInit {
  blogs: any = [];
  blogCategory: any;
  mostReads: any = [];
  blogCategories: any = [];
  total: number = 0;
  skip: number = 0;
  pageSize: number = 4;
  environment = environment;
  url = '';

  constructor(private appService: AppService, private router: ActivatedRoute) {}

  ngOnInit(): void {
    this.router.params.subscribe((params) => {
      this.url = params['url'];
      this.getBlogs();
      this.getByUrl();
    });
    this.getMostRead();
    this.getBlogCategories();
  }

  getBlogs() {
    this.appService
      .get(`Blog/GetBlogsByCategoryUrl/${this.url}?$count=true`)
      .subscribe((res: any) => {
        this.blogs = res.data;
        this.total = res.total;
      });
  }

  getByUrl() {
    this.appService.get(`BlogCategory/GetByUrl/${this.url}`).subscribe((res: any) => {
      this.blogCategory = res;
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
