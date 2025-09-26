import { Component, OnInit } from '@angular/core';
import { SharedModule } from '../../shared.module';
import { AppService } from '../../services/app.service';
import {
  FormGroup,
  FormBuilder,
  AbstractControl,
  FormControl,
  Validators,
} from '@angular/forms';
import { AlertService, alertType } from '../../services/alert.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
  imports: [SharedModule],
})
export class DashboardComponent implements OnInit {
  taskForm: FormGroup = this.formBuilder.group({
    id: [0],
    title: [],
    taskCategoryId: new FormControl({ value: null, disabled: true }),
    taskStatusId: [null, [Validators.required]],
  });
  data: any = {};
  selectedData: any = {};
  tasks: any = [];
  gridMenuItems: any = [];
  taskCategories: any = [];
  taskStatuses: any = [];
  loadingSummary = false;
  loading = false;
  visibleTaskModal = false;
  submittedTaskForm = false;
  rows = 5;
  first = 0;
  totalRecords = 0;
  description = '';

  constructor(
    private appService: AppService,
    private formBuilder: FormBuilder,
    private alertService: AlertService
  ) {}

  ngOnInit() {
    this.getCount();
    this.getUserTasks();
    this.gridMenuItems = [
      {
        label: 'Edit',
        command: (e: any) => {
          this.edit(this.selectedData.id);
        },
      },
    ];
  }

  getCount() {
    this.loadingSummary = true;
    this.appService.get('Dashboard/GetCount').subscribe((res: any) => {
      this.data = res;
      this.loadingSummary = false;
    });
  }

  getUserTasks() {
    this.loading = true;
    this.appService
      .get(
        `Task/GetUserTasks?$skip=${this.first}&$top=${this.rows}&$count=true&$orderby=updatedDate desc,insertedDate desc`
      )
      .subscribe((res: any) => {
        this.tasks = res.data;
        this.totalRecords = res.total;
        this.loading = false;
      });
  }

  getTaskCategories() {
    this.appService.get('Lookup/TaskCategories').subscribe((res: any) => {
      this.taskCategories = res;
    });
  }

  get fTask(): { [key: string]: AbstractControl } {
    return this.taskForm.controls;
  }

  pageChange(e: any) {
    this.first = e.first;
    this.rows = e.rows;
    this.getUserTasks();
  }

  menuToggle(menu: any, e: any, data: any) {
    this.gridMenuItems.forEach((menuItem: any) => {
      menuItem.data = data;
    });
    this.selectedData = data;
    menu.toggle(e);
  }

  edit(id: number) {
    this.description = '';
    this.getTaskCategories();
    this.appService.get(`Task/GetById/${id}`).subscribe((res: any) => {
      if (res.taskCategoryId) {
        this.appService
          .get(`Lookup/TaskStatuses/${res.taskCategoryId}`)
          .subscribe((status: any) => {
            this.taskForm.patchValue(res);
            this.taskStatuses = status;
            this.description = res.description;
            this.visibleTaskModal = true;
          });
      } else {
        this.taskForm.patchValue(res);
        this.description = res.description;
        this.visibleTaskModal = true;
      }
    });
  }

  saveTask() {
    this.submittedTaskForm = true;
    if (this.taskForm?.invalid) {
      return;
    }
    var value = this.taskForm.value;
    var data = {
      id: value.id,
      taskStatusId: value.taskStatusId,
    };
    this.appService.put('Task/UpdateUserTask', data).subscribe((res: any) => {
      this.getUserTasks();
      this.alertService.showMessage(
        'The update was successful',
        alertType.success
      );
      this.visibleTaskModal = false;
    });
  }
}
