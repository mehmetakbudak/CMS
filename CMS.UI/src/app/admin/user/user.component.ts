import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css'],
})
export class UserComponent implements OnInit {
  form!: FormGroup;
  list: any = [];
  userTypes: any = [];
  loading = false;
  total = 0;
  pageNumber = 1;
  pageSize = 5;
  visibleModal = false;
  titleModal = '';

  constructor(
    private http: HttpClient,
    private fb: FormBuilder,
    private notification: NzNotificationService
  ) {}

  ngOnInit() {
    this.form = this.fb.group({
      id: [0],
      emailAddress: [null, [Validators.email, Validators.required]],
      phone: [],
      name: [null, [Validators.required]],
      surname: [null, [Validators.required]],
      userType: [null, [Validators.required]],
      isActive: [true, [Validators.required]],
      status: [],
      userTypeName: [],
    });
    this.getUserTypes();
  }

  getAll() {
    this.http
      .get(
        `${environment.ApiUrl}adminuser?pageNumber=${this.pageNumber}&pageSize=${this.pageSize}`
      )
      .subscribe((res: any) => {
        this.list = res.data;
        this.total = res.total;
        this.pageNumber = res.pageNumber;
        this.pageSize = res.pageSize;
      });
  }

  getUserTypes() {
    this.http
      .get(`${environment.ApiUrl}adminlookup/usertypes`)
      .subscribe((res: any) => {
        this.userTypes = res;
      });
  }

  onQueryParamsChange(e: any) {
    this.pageNumber = e.pageIndex;
    this.pageSize = e.pageSize;
    this.getAll();
  }

  add() {
    this.visibleModal = true;
    this.titleModal = 'Kullanıcı Ekle';
    this.form.reset();
    this.form.patchValue({
      id: 0,
      isActive: true,
    });
  }

  edit(id: number) {
    this.visibleModal = true;
    this.titleModal = 'Kullanıcı Düzenle';
    this.http
      .get(`${environment.ApiUrl}adminuser/${id}`)
      .subscribe((res: any) => {
        this.form.setValue(res);
      });
  }

  deleteUser(id: number) {
    this.http
      .delete(`${environment.ApiUrl}adminuser/${id}`)
      .subscribe((res: any) => {
        this.visibleModal = false;
        this.getAll();
        this.notification.create('success', 'İşlem Başarılı', res.message);
      });
  }

  save() {
    if (this.form.valid) {
      if (this.form.value.id) {
        this.http
          .put(`${environment.ApiUrl}adminuser`, this.form.value)
          .subscribe((res: any) => {
            this.visibleModal = false;
            this.getAll();
            this.notification.create('success', 'İşlem Başarılı', res.message);
          });
      } else {
        this.http
          .post(`${environment.ApiUrl}adminuser`, this.form.value)
          .subscribe((res: any) => {
            this.visibleModal = false;
            this.getAll();
            this.notification.create('success', 'İşlem Başarılı', res.message);
          });
      }
    } else {
      Object.values(this.form.controls).forEach((control) => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }
}
