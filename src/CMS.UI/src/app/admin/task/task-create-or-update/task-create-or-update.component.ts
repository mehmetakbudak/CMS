import { AlertService, alertType } from './../../../services/alert.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from '../../../services/app.service';
import { SharedModule } from '../../../shared.module';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';

@Component({
  selector: 'app-task-create-or-update',
  imports: [SharedModule],
  templateUrl: './task-create-or-update.component.html',
  styleUrl: './task-create-or-update.component.css',
})
export class TaskCreateOrUpdateComponent implements OnInit {
  form: FormGroup = this.formBuilder.group({
    id: [0, []],
    taskCategoryId: [null, Validators.required],
    taskStatusId: [null, Validators.required],
    title: ['', [Validators.required]],
    assignUserId: [null, []],
    isActive: [true, [Validators.required]],
    description: ['', []],
  });
  id: number = 0;
  taskCategories: any = [];
  taskStatuses: any = [];
  users: any = [];
  submitted = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private appService: AppService,
    private formBuilder: FormBuilder,
    private alertService: AlertService
  ) {}

  ngOnInit() {
    this.getTaskCategories();
    this.getAdminUsers();
    this.route.params.subscribe((params) => {
      this.id = params['id'];
      if (this.id > 0) {
        this.appService
          .get(`Task/GetById/${this.id}`)
          .subscribe((res: any) => {
            this.form.patchValue(res);
            this.getTaskStatuses(res.taskCategoryId);
          });
      }
    });
  }

  getTaskCategories() {
    this.appService.get(`Lookup/TaskCategories`).subscribe((res: any) => {
      this.taskCategories = res;
    });
  }

  getTaskStatuses(id: number) {
    this.appService.get(`Lookup/TaskStatuses/${id}`).subscribe((res: any) => {
      this.taskStatuses = res;
    });
  }

  onCategoryChange(event: any) {
    this.getTaskStatuses(event.value);
  }

  getAdminUsers() {
    this.appService.get(`Lookup/Users`).subscribe((res: any) => {
      this.users = res;
    });
  }

  get f(): { [key: string]: AbstractControl } {
    return this.form.controls;
  }

  save() {
    this.submitted = true;
    if (this.form?.invalid) {
      return;
    }
    if (this.form.value.id == 0) {
      this.appService
        .post(`Task/Create`, this.form.value)
        .subscribe((res: any) => {
          this.alertService.showMessage(
            'Addition was successful',
            alertType.success
          );
          setTimeout(() => {
            this.router.navigateByUrl(`/admin/task`);
          }, 1000);
        });
    } else {
      this.appService
        .put(`Task/Update`, this.form.value)
        .subscribe((res: any) => {
          this.alertService.showMessage(
            'The update was successful',
            alertType.success
          );
          setTimeout(() => {
            this.router.navigateByUrl(`/admin/task`);
          }, 1000);
        });
    }
  }
}
