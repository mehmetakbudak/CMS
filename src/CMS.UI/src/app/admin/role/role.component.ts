import { AlertService, alertType } from '../../services/alert.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { GetActiveStatus } from '../../models/consts';
import { AppService } from '../../services/app.service';
import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { SharedModule } from '../../shared.module';

class RoleFilter {
  name: string = '';
  isActive: boolean | null = null;
}

@Component({
  selector: 'app-role',
  templateUrl: './role.component.html',
  styleUrl: './role.component.css',
  imports: [SharedModule],
})
export class RoleComponent implements OnInit {
  form: FormGroup = this.formBuilder.group({
    id: [0],
    name: ['', Validators.required],
    isActive: [true, Validators.required],
    accessRightIds: [[]],
  });
  roles: any = [];
  accessRightCategories: any = [];
  totalRecords!: number;
  rows = 5;
  first = 0;
  menuItems: any = [];
  gridMenuItems: any = [];
  loading = false;
  filterForm!: RoleFilter;
  selectedData: any;
  activeStatus: any = [];
  submitted = false;
  isVisible = false;
  title = '';
  selectedAccessRights: any = [];

  constructor(
    private appService: AppService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private formBuilder: FormBuilder,
    private alertService: AlertService
  ) {}

  ngOnInit() {
    this.filterForm = new RoleFilter();
    this.activeStatus = GetActiveStatus();
    this.getRoles();
    this.menuItems = [
      {
        label: 'Add',
        command: () => {
          this.form.reset();
          this.isVisible = true;
          this.form.patchValue({ id: 0, isActive: true });
          this.title = 'Add Role';
          this.getAccessRightsWithCategory(null);
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

  edit(id: any) {
    this.getAccessRightsWithCategory(id);
    this.appService.get(`Role/GetById/${id}`).subscribe((res: any) => {
      this.form.patchValue(res);
      this.isVisible = true;
      this.title = 'Edit Role';
    });
  }

  getRoles() {
    this.loading = true;
    let filter = '';
    let conditionals = [];
    if (this.filterForm.name != '') {
      conditionals.push(`contains(tolower(name), '${this.filterForm.name}') `);
    }
    if (this.filterForm.isActive != null) {
      conditionals.push(`isActive eq ${this.filterForm.isActive}`);
    }
    if (conditionals.length > 1) {
      filter = '$filter=';
      for (let i = 0; i < conditionals.length; i++) {
        if (i == conditionals.length - 1) {
          filter += conditionals[i];
        } else {
          filter += conditionals[i] + ' and ';
        }
      }
      filter += '&';
    } else if (conditionals.length == 1) {
      filter = `$filter=${conditionals[0]}&`;
    }
    this.appService
      .get(
        `Role/Get?${filter}$skip=${this.first}&$top=${this.rows}&$count=true&$orderby=id desc`
      )
      .subscribe((res: any) => {
        this.roles = res.data;
        this.totalRecords = res.total;
        this.loading = false;
      });
  }

  getAccessRightsWithCategory(id: any) {
    var url = id
      ? `AccessRight/GetAccessRightsWithCategory?roleId=${id}`
      : 'AccessRight/GetAccessRightsWithCategory';
    this.appService.get(url).subscribe((res: any) => {
      this.accessRightCategories = res;
      this.selectedAccessRights = [];
      res.forEach((x: any) => {
        x.items.forEach((y: any) => {
          if (y.selected) {
            this.selectedAccessRights.push(y.value);
          }
        });
      });
    });
  }

  get f(): { [key: string]: AbstractControl } {
    return this.form.controls;
  }

  menuToggle(menu: any, e: any, data: any) {
    this.gridMenuItems.forEach((menuItem: any) => {
      menuItem.data = data;
    });
    this.selectedData = data;
    menu.toggle(e);
  }

  reset() {
    this.filterForm = new RoleFilter();
    this.getRoles();
  }

  search() {
    this.getRoles();
  }

  pageChange(e: any) {
    this.first = e.first;
    this.rows = e.rows;
    this.getRoles();
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
          .delete(`Role/Delete`, this.selectedData.id)
          .subscribe((res: any) => {
            this.getRoles();
            this.messageService.add({
              severity: 'info',
              summary: 'Success',
              detail: 'Deletion was successful',
            });
          });
      },
    });
  }

  save() {
    this.submitted = true;
    if (this.selectedAccessRights.length == 0) {
      this.alertService.showMessage(
        'Please select at least one access right!',
        alertType.error
      );
      return;
    }
    this.form.patchValue({
      accessRightIds: this.selectedAccessRights,
    });
    if (this.form?.invalid) {
      return;
    }
    if (this.form.value.id == 0) {
      this.appService.post('Role/Create', this.form.value).subscribe((res: any) => {
        this.getRoles();
        this.alertService.showMessage(
          'Addition was successful',
          alertType.success
        );
        this.isVisible = false;
      });
    } else {
      this.appService.put('Role/Update', this.form.value).subscribe((res: any) => {
        this.getRoles();
        this.alertService.showMessage(
          'The update was successful',
          alertType.success
        );
        this.isVisible = false;
      });
    }
  }
}
