import { ConfirmationService } from 'primeng/api';
import { AppService } from './../../services/app.service';
import { Component, OnInit } from '@angular/core';
import { AlertService, alertType } from '../../services/alert.service';
import { SharedModule } from '../../shared.module';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrl: './contact.component.css',
  imports: [SharedModule],
})
export class ContactComponent implements OnInit {
  list: any = [];
  rows = 5;
  first = 0;
  totalRecords = 0;
  menuItems: any = [];
  gridMenuItems: any = [];
  selectedData: any;
  loading = false;

  constructor(
    private appService: AppService,
    private confirmationService: ConfirmationService,
    private alertService: AlertService
  ) {}

  ngOnInit() {
    this.getList();
    this.gridMenuItems = [
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
                .delete('Contact/Delete', this.selectedData.id)
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

  getList() {
    this.loading = true;
    this.appService
      .get(`Contact/Get?$skip=${this.first}&$top=${this.rows}&$count=true`)
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

  search() {}

  reset() {}
}
