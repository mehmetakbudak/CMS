import { Component, OnInit } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { AppService } from '../../../services/app.service';
import { Router } from '@angular/router';
import { SharedModule } from '../../../shared.module';

class UserFilter {
  id!: number;
  name!: string;
  surname!: string;
  isActive: boolean | null = null;
}

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss'],
  imports: [SharedModule],
})
export class UserListComponent implements OnInit {
  totalRecords!: number;
  rows = 5;
  first = 0;
  menuItems: any = [];
  gridMenuItems: any = [];
  loading = false;
  selectedData: any = null;
  filterForm!: UserFilter;
  users: any = [];

  constructor(
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private appService: AppService,
    private router: Router
  ) {}

  ngOnInit() {
    this.filterForm = new UserFilter();
    this.getUsers();
    this.menuItems = [
      {
        label: 'Add',
        command: () => {
          this.router.navigateByUrl(`/admin/user/create`);
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
                .delete(`User`, this.selectedData.id)
                .subscribe((res: any) => {
                  this.getUsers();
                  this.messageService.add({
                    severity: 'info',
                    summary: 'Success',
                    detail: 'Deletion was successful',
                  });
                });
            },
          });
        },
      },
    ];
  }

  edit(id: any) {
    this.router.navigateByUrl(`/admin/user/update/${id}`);
  }

  getUsers() {
    let filter = '';

    this.appService
      .get(
        `User/Get?${filter}$skip=${this.first}&$top=${this.rows}&$count=true&$orderby=updatedDate desc,insertedDate desc`
      )
      .subscribe((res: any) => {
        this.users = res.data;
        this.totalRecords = res.total;
      });
  }

  pageChange(e: any) {
    this.first = e.first;
    this.rows = e.rows;
    this.getUsers();
  }

  menuToggle(menu: any, e: any, data: any) {
    this.gridMenuItems.forEach((menuItem: any) => {
      menuItem.data = data;
    });
    this.selectedData = data;
    menu.toggle(e);
  }

  reset() {
    this.filterForm = new UserFilter();
    this.getUsers();
  }

  search() {
    this.getUsers();
  }
}
