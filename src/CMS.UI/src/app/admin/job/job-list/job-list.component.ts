import { Component, OnInit } from '@angular/core';
import { AppService } from '../../../services/app.service';
import { Router } from '@angular/router';
import { ConfirmationService } from 'primeng/api';
import { AlertService, alertType } from '../../../services/alert.service';
import { SharedModule } from '../../../shared.module';

@Component({
  selector: 'app-job-list',
  templateUrl: './job-list.component.html',
  styleUrl: './job-list.component.css',
  imports: [SharedModule],
})
export class JobListComponent implements OnInit {
  list: any = [];
  rows = 5;
  first = 0;
  totalRecords = 0;
  menuItems: any = [];
  gridMenuItems: any = [];
  selectedData: any;
  loading = false;
  workTypes: any = [];

  constructor(
    private appService: AppService,
    private router: Router,
    private confirmationService: ConfirmationService,
    private alertService: AlertService
  ) {}

  ngOnInit() {
    this.getList();
    this.getWorkTypes();
    this.menuItems = [
      {
        label: 'Add',
        command: () => {
          this.router.navigateByUrl('/admin/job/create');
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
                .delete('Job/Delete', this.selectedData.id)
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

  edit(id: any) {
    this.router.navigateByUrl(`/admin/job/update/${id}`);
  }

  getList() {
    this.loading = true;
    this.appService
      .get(
        `Job/Get?$skip=${this.first}&$top=${this.rows}&$count=true&$orderBy=updatedDate desc,insertedDate desc`
      )
      .subscribe((res: any) => {
        this.loading = false;
        this.list = res.data;
        this.totalRecords = res.total;
      });
  }

  getWorkTypes() {
    this.appService.get('Lookup/WorkTypes').subscribe((res: any) => {
      this.workTypes = res;
    });
  }

  getWorkTypeName(id: any) {
    const workType = this.workTypes.find((x: any) => x.id === id);
    return workType ? workType.name : '';
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

  reset() {
    this.getList();
  }

  search() {}
}
