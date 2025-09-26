import { UserType } from './../../../models/enums';
import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from '../../../services/app.service';
import { AlertService, alertType } from '../../../services/alert.service';
import { SharedModule } from '../../../shared.module';

@Component({
  selector: 'app-user-create-or-update',
  templateUrl: './user-create-or-update.component.html',
  styleUrl: './user-create-or-update.component.css',
  imports: [SharedModule],
})
export class UserCreateOrUpdateComponent implements OnInit {
  form: FormGroup = this.formBuilder.group({
    id: [0, []],
    name: ['', Validators.required],
    surname: ['', [Validators.required]],
    emailAddress: ['', [Validators.required, Validators.email]],
    phone: [null, [Validators.required]],
    status: [null],
    userType: [null, Validators.required],
    roleIds: [[]],
    isActive: [true, Validators.required],
  });
  id: number = 0;
  submitted = false;
  userStatuses: any = [];
  userTypes: any = [];
  roles: any = [];
  UserType = UserType;

  constructor(
    private formBuilder: FormBuilder,
    private appService: AppService,
    private alertService: AlertService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    this.getRoles();
    this.getUserTypes();
    this.getUserStatuses();
    this.route.params.subscribe((params) => {
      this.id = params['id'];
      if (this.id > 0) {
        this.appService.get(`User/GetById/${this.id}`).subscribe((res: any) => {
          this.form.patchValue(res);
        });
      }
    });
  }

  get f(): { [key: string]: AbstractControl } {
    return this.form.controls;
  }

  getUserStatuses() {
    this.appService.get('Lookup/UserStatuses').subscribe((res: any) => {
      this.userStatuses = res;
    });
  }

  getUserTypes() {
    this.appService.get('Lookup/UserTypes').subscribe((res: any) => {
      this.userTypes = res;
    });
  }

  getRoles() {
    this.appService.get('Lookup/Roles').subscribe((res: any) => {
      this.roles = res;
    });
  }

  save() {
    this.submitted = true;
    if (this.form?.invalid) {
      return;
    }
    if (this.form.value.id == 0) {
      this.appService.post(`User/Create`, this.form.value).subscribe((res: any) => {
        this.alertService.showMessage(
          'Addition was successful',
          alertType.success
        );
        setTimeout(() => {
          this.router.navigateByUrl(`/admin/user`);
        }, 1000);
      });
    } else {
      this.appService.put(`User/Update`, this.form.value).subscribe((res: any) => {
        this.alertService.showMessage(
          'The update was successful',
          alertType.success
        );
        setTimeout(() => {
          this.router.navigateByUrl(`/admin/user`);
        }, 1000);
      });
    }
  }
}
