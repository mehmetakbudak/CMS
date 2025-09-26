import { Component, OnInit } from '@angular/core';
import { AppService } from '../../services/app.service';
import { SharedModule } from '../../shared.module';
import { AlertService, alertType } from '../../services/alert.service';
import { ConfirmationService } from 'primeng/api';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrl: './comment.component.css',
  imports: [SharedModule],
})
export class CommentComponent implements OnInit {
  list: any = [];
  rows = 5;
  first = 0;
  totalRecords = 0;
  gridMenuItems: any = [];
  selectedData: any;
  loading = false;
  commentStatuses: any = [];

  constructor(
    private appService: AppService,
    private alertService: AlertService,
    private confirmationService: ConfirmationService
  ) {}

  ngOnInit() {
    this.getList();
    this.getCommentStatus();
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
                .delete(`Comment/Delete`, this.selectedData.id)
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
      .get(
        `Comment/Get?$skip=${this.first}&$top=${this.rows}&$count=true&$orderBy=insertedDate desc`
      )
      .subscribe((res: any) => {
        this.loading = false;
        this.list = res.data;
        this.totalRecords = res.total;
      });
  }

  getCommentStatus() {
    this.appService.get('Lookup/CommentStatuses').subscribe((res: any) => {
      this.commentStatuses = res;
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

  updateComment(comment: any) {
    this.appService
      .put('Comment/Update', {
        id: comment.id,
        commentStatus: comment.statusId,
      })
      .subscribe((res: any) => {
        this.alertService.showMessage(
          'The update was successful',
          alertType.success
        );
        this.getList();
      });
  }
}
