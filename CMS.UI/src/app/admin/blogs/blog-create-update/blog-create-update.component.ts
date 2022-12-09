import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-blog-create-update',
  templateUrl: './blog-create-update.component.html',
  styleUrls: ['./blog-create-update.component.css'],
})
export class BlogCreateUpdateComponent implements OnInit {
  pageTitle = '';

  constructor(private route: ActivatedRoute) {}

  ngOnInit() {
    var routeParams: any = this.route.params;
    if (routeParams.value.id) {
      this.pageTitle = 'Blog Düzenle';
    } else {
      this.pageTitle = 'Blog Ekle';
    }
  }
}
