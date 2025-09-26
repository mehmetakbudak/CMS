import { Component, OnInit } from '@angular/core';
import { SharedModule } from '../../../shared.module';
import { AppService } from '../../../services/app.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { TableModule } from 'primeng/table';
import { MenuModule } from 'primeng/menu';
import { ButtonModule } from 'primeng/button';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { PanelModule } from 'primeng/panel';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ToastModule } from 'primeng/toast';
import { AlertService, alertType } from '../../../services/alert.service';

@Component({
  selector: 'app-user-applied-job',
  imports: [
    CommonModule,
    TableModule,
    MenuModule,
    ButtonModule,
    RouterLink,
    PanelModule,
    ConfirmDialogModule,
    ToastModule,
  ],
  providers: [ConfirmationService, AlertService, MessageService],
  templateUrl: './user-applied-job.component.html',
  styleUrl: './user-applied-job.component.css',
})
export class UserAppliedJobComponent implements OnInit {
  list: any = [];
  gridMenuItems: any = [];
  selectedData: any = {};
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
                .delete(`UserJob/Delete`, this.selectedData.id)
                .subscribe((res: any) => {
                  this.getList();
                  this.alertService.showMessage(
                    'Deleted successfully',
                    alertType.success
                  );
                });
            },
          });
        },
      },
    ];
  }

  menuToggle(menu: any, e: any, data: any) {
    this.gridMenuItems.forEach((menuItem: any) => {
      menuItem.data = data;
    });
    this.selectedData = data;
    menu.toggle(e);
  }

  getList() {
    this.loading = true;
    this.appService.get('UserJob/GetAppliedJobs').subscribe((res: any) => {
      this.list = res;
      this.loading = false;
    });
  }
}
