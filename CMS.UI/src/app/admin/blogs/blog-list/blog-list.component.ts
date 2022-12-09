import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-blog-list',
  templateUrl: './blog-list.component.html',
  styleUrls: ['./blog-list.component.css'],
})
export class BlogListComponent implements OnInit {
  expandSet = new Set<number>();
  list: any = [];
  loading = false;
  total = 0;
  pageSize = 0;
  visibleModal = false;
  modalWidth = '50vw';
  titleModal = '';

  constructor(private http: HttpClient, private router: Router) {}

  ngOnInit() {
    this.getAll();
    this.modalWidth = window.innerWidth > 768 ? '50vw' : '95vw';
  }

  getAll() {
    this.http.get(`${environment.ApiUrl}adminblog`).subscribe((res: any) => {
      this.list = res;
    });
  }

  onExpandChange(id: number, checked: boolean): void {
    if (checked) {
      this.expandSet.add(id);
    } else {
      this.expandSet.delete(id);
    }
  }

  edit(id: number) {
    this.router.navigateByUrl(`/admin/blog/edit/${id}`);
  }

  deleteBlog() {}
}
