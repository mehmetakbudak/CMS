import { SharedModule } from './../../../shared.module';
import { Component, OnInit } from '@angular/core';
import { AppService } from '../../../services/app.service';
import { ConfirmationService } from 'primeng/api';
import { UserFileType } from '../../../models/enums';
import { AlertService, alertType } from '../../../services/alert.service';

@Component({
  selector: 'app-my-cv',
  imports: [SharedModule],
  templateUrl: './my-cv.component.html',
  styleUrl: './my-cv.component.css',
})
export class MyCvComponent implements OnInit {
  list: any = [];
  gridMenuItems: any = [];
  selectedData: any = {};
  showAddModal = false;
  isDefault = true;
  uploadFile: any = {};
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
        label: 'Set Default',
        command: (e: any) => {
          this.appService
            .put(`UserFile/SetDefault/${this.selectedData.id}`, {})
            .subscribe((res: any) => {
              this.getList();
              this.alertService.showMessage(
                'Default set successfully',
                alertType.success
              );
            });
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
                .delete(`UserFile/Delete`, this.selectedData.id)
                .subscribe(
                  (res: any) => {
                    this.getList();
                    this.alertService.showMessage(
                      'Deleted successfully',
                      alertType.success
                    );
                  },
                  (err: any) => {
                    this.alertService.showMessage(
                      err.error.message,
                      alertType.error
                    );
                  }
                );
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
    this.appService
      .get('UserFile/GetUserFiles?$orderBy=updatedDate desc,insertedDate desc')
      .subscribe((res: any) => {
        this.list = res.data;
        this.loading = false;
      });
  }

  add() {
    this.showAddModal = true;
  }

  onSelect(e: any) {
    this.uploadFile = e.files[0];
  }

  save() {
    if (this.uploadFile) {
      var file = new FormData();
      file.append('file', this.uploadFile);
      file.append('isDefault', this.isDefault.toString());
      file.append('fileType', UserFileType.CV.toString());
      this.appService.post('UserFile/Create', file).subscribe(
        (res: any) => {
          this.showAddModal = false;
          this.getList();
          this.alertService.showMessage(
            'Saved successfully',
            alertType.success
          );
        },
        (err: any) => {
          this.alertService.showMessage(err.error.message, alertType.error);
        }
      );
    }
  }
}
