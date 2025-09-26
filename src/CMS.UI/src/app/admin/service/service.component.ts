import { environment } from './../../../environments/environment';
import { Component, OnInit } from '@angular/core';
import { AppService } from '../../services/app.service';
import { SharedModule } from '../../shared.module';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { AlertService, alertType } from '../../services/alert.service';
import { ConfirmationService } from 'primeng/api';

@Component({
  selector: 'app-service',
  templateUrl: './service.component.html',
  styleUrl: './service.component.css',
  imports: [SharedModule],
})
export class ServiceComponent implements OnInit {
  form: FormGroup = this.formBuilder.group({
    id: [0],
    name: ['', Validators.required],
    url: new FormControl({ value: '', disabled: true }),
    isActive: [true, Validators.required],
  });
  list: any = [];
  rows = 5;
  first = 0;
  totalRecords = 0;
  menuItems: any = [];
  gridMenuItems: any = [];
  selectedData: any;
  loading = false;
  submitted = false;
  isVisible = false;
  title = '';
  imageUrl = '';
  content = '';
  environment = environment;
  file: any = null;

  constructor(
    private appService: AppService,
    private formBuilder: FormBuilder,
    private alertService: AlertService,
    private confirmationService: ConfirmationService
  ) {}

  ngOnInit() {
    this.getList();
    this.menuItems = [
      {
        label: 'Add',
        command: () => {
          this.form.reset();
          this.isVisible = true;
          this.form.patchValue({ id: 0, isActive: true });
          this.title = 'Add Service';
          this.content = '';
          this.imageUrl = '';
        },
      },
    ];
    this.gridMenuItems = [
      {
        label: 'Edit',
        command: () => {
          this.edit(this.selectedData.id);
        },
      },
      {
        label: 'Delete',
        command: (e: any) => {
          this.confirmationService.confirm({
            target: e.target as EventTarget,
            message: 'Are you sure that you want to delete?',
            header: 'Delete',
            icon: 'pi pi-exclamation-triangle',
            acceptIcon: 'none',
            rejectIcon: 'none',
            rejectButtonStyleClass: 'p-button-text',
            accept: () => {
              this.appService
                .delete(`Service/Delete`, this.selectedData.id)
                .subscribe((res: any) => {
                  this.getList();
                  this.alertService.showMessage(
                    'Deletion was successful',
                    alertType.success
                  );
                });
            },
          });
        },
      },
    ];
  }

  get f(): { [key: string]: AbstractControl } {
    return this.form.controls;
  }

  getList() {
    this.loading = true;
    this.appService
      .get(
        `Service/Get?$skip=${this.first}&$top=${this.rows}&$count=true&$orderby=id desc`
      )
      .subscribe((res: any) => {
        this.loading = false;
        this.list = res.data;
        this.totalRecords = res.total;
      });
  }

  pageChange(e: any) {
    this.first = e.first;
    this.rows = e.rows;
    this.getList();
  }

  menuToggle(menu: any, e: any, data: any) {
    this.gridMenuItems.forEach((menuItem: any) => {
      menuItem.data = data;
    });
    this.selectedData = data;
    menu.toggle(e);
  }

  edit(id: number) {
    this.appService.get(`Service/GetById/${id}`).subscribe((res: any) => {
      this.form.patchValue(res);
      this.isVisible = true;
      this.title = 'Edit Service';
      this.content = res.content;
      this.imageUrl = res.imageUrl;
    });
  }

  onSelect(e: any) {
    this.file = e.files[0];
  }

  reset() {}

  search() {}

  save() {
    this.submitted = true;
    if (this.form?.invalid) {
      return;
    }
    var data = this.form.value;
    var formData = new FormData();
    formData.append('id', data.id);
    formData.append('name', data.name);
    formData.append('content', this.content);
    formData.append('isActive', data.isActive);
    formData.append('image', this.file);
    if (data.id == 0) {
      this.appService.post('Service/Create', formData).subscribe((res: any) => {
        this.alertService.showMessage(
          'Addition was successful',
          alertType.success
        );
        this.isVisible = false;
        this.getList();
      });
    } else {
      this.appService.put('Service/Update', formData).subscribe((res: any) => {
        this.alertService.showMessage(
          'The update was successful',
          alertType.success
        );
        this.isVisible = false;
        this.getList();
      });
    }
  }
}
