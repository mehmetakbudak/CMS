import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ConfirmationService } from 'primeng/api';
import { AppService } from '../../services/app.service';
import { AlertService, alertType } from '../../services/alert.service';
import { SharedModule } from '../../shared.module';

class BlogCategoryFilter {
  name!: string;
  url!: string;
  isActive: boolean | null = null;
}

@Component({
  selector: 'app-blog-category',
  templateUrl: './blog-category.component.html',
  standalone: true,
  imports: [SharedModule],
})
export class BlogCategoryComponent implements OnInit {
  form: FormGroup = this.formBuilder.group({
    id: [0],
    name: ['', Validators.required],
    url: ['', [Validators.required]],
    isActive: [true, Validators.required],
  });
  totalRecords!: number;
  rows = 5;
  first = 0;
  menuItems: any = [];
  gridMenuItems: any = [];
  loading = false;
  selectedData!: any;
  filterForm!: BlogCategoryFilter;
  submitted = false;
  blogCategories: any = [];
  isVisible = false;
  title = '';

  constructor(
    private alertService: AlertService,
    private confirmationService: ConfirmationService,
    private appService: AppService,
    private formBuilder: FormBuilder
  ) {}

  ngOnInit() {
    this.filterForm = new BlogCategoryFilter();
    this.getBlogCategories();
    this.menuItems = [
      {
        label: 'Add',
        command: () => {
          this.form.reset();
          this.isVisible = true;
          this.form.patchValue({ id: 0, isActive: true });
          this.title = 'Add Blog Category';
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
          this.delete(e);
        },
      },
    ];
  }

  getBlogCategories() {
    let filter = '';

    this.appService
      .get(
        `BlogCategory/Get?${filter}$skip=${this.first}&$top=${this.rows}&$count=true&$orderby=id desc`
      )
      .subscribe((res: any) => {
        this.blogCategories = res.data;
        this.totalRecords = res.total;
      });
  }

  get f(): { [key: string]: AbstractControl } {
    return this.form.controls;
  }

  pageChange(e: any) {
    this.first = e.first;
    this.rows = e.rows;
    this.getBlogCategories();
  }

  menuToggle(menu: any, e: any, data: any) {
    this.gridMenuItems.forEach((menuItem: any) => {
      menuItem.data = data;
    });
    this.selectedData = data;
    menu.toggle(e);
  }

  reset() {
    this.filterForm = new BlogCategoryFilter();
    this.getBlogCategories();
  }

  search() {
    this.getBlogCategories();
  }

  edit(id: any) {
    this.appService.get(`BlogCategory/GetById/${id}`).subscribe((res: any) => {
      this.form.patchValue(res);
      this.isVisible = true;
      this.title = 'Edit Blog Category';
    });
  }

  delete(event: any) {
    this.confirmationService.confirm({
      target: event.target as EventTarget,
      message: 'Are you sure that you want to delete?',
      header: 'Delete',
      icon: 'pi pi-exclamation-triangle',
      acceptIcon: 'none',
      rejectIcon: 'none',
      rejectButtonStyleClass: 'p-button-text',
      accept: () => {
        this.appService
          .delete(`BlogCategory/Delete`, this.selectedData.id)
          .subscribe((res: any) => {
            this.getBlogCategories();
            this.alertService.showMessage(
              'Deletion was successful',
              alertType.success
            );
          });
      },
    });
  }

  save() {
    this.submitted = true;
    if (this.form?.invalid) {
      return;
    }
    if (this.form.value.id == 0) {
      this.appService
        .post('BlogCategory/Create', this.form.value)
        .subscribe((res: any) => {
          this.getBlogCategories();
          this.alertService.showMessage(
            'Addition was successful',
            alertType.success
          );
          this.isVisible = false;
        });
    } else {
      this.appService
        .put('BlogCategory/Update', this.form.value)
        .subscribe((res: any) => {
          this.getBlogCategories();
          this.alertService.showMessage(
            'The update was successful',
            alertType.success
          );
          this.isVisible = false;
        });
    }
  }
}
