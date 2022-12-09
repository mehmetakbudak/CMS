import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-blog-list',
  templateUrl: './blog-list.component.html',
  styleUrls: ['./blog-list.component.css'],
})
export class BlogListComponent implements OnInit {
  blogCategories: any = [];
  blogs: any = [];
  environment: any;

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.getBlogs();
    this.getBlogCategories();
    this.environment = environment;
  }

  getBlogs() {
    this.http.get(`${environment.ApiUrl}blog`).subscribe((res: any) => {
      this.blogs = res;
    });
  }

  getBlogCategories() {
    this.http.get(`${environment.ApiUrl}blogcategory`).subscribe((res: any) => {
      this.blogCategories = res;
    });
  }
}
