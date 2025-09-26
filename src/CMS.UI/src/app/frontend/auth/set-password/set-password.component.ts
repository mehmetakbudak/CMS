import { Component, OnInit } from '@angular/core';
import { SharedModule } from '../../../shared.module';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { AppService } from '../../../services/app.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertService, alertType } from '../../../services/alert.service';

@Component({
  selector: 'app-set-password',
  imports: [SharedModule],
  templateUrl: './set-password.component.html',
  styleUrl: './set-password.component.css',
})
export class SetPasswordComponent implements OnInit {
  form: FormGroup = this.formBuilder.group({
    code: [''],
    newPassword: ['', [Validators.required]],
    reNewPassword: ['', [Validators.required]],
  });
  submitted = false;
  disabled = false;
  loading = false;
  code = null;
  userFullName = '';

  constructor(
    private formBuilder: FormBuilder,
    private alertService: AlertService,
    private appService: AppService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    this.route.params.subscribe((params: any) => {
      this.code = params['code'];
      if (this.code) {
        this.appService.get('Auth/GetUserByCode?code=' + this.code).subscribe({
          next: (res: any) => {
            this.userFullName = res.fullName;
          },
          error: (err: any) => {
            this.router.navigateByUrl('/');
          },
        });
      } else {
        this.router.navigateByUrl('/');
      }
    });
  }

  get f(): { [key: string]: AbstractControl } {
    return this.form.controls;
  }

  onSubmit() {
    this.submitted = true;
    if (this.form?.invalid) {
      return;
    }
    this.disabled = true;
    this.form.patchValue({
      code: this.code,
    });
    this.appService.post('Auth/ResetPassword', this.form.value).subscribe({
      next: (res: any) => {
        this.alertService.showMessage(
          this.appService.getTranslate('SetPassword.SuccessMessage'),
          alertType.success
        );
        setTimeout(() => {
          this.router.navigateByUrl('/');
        }, 1000);
      },
      error: (err: any) => {
        this.alertService.showMessage(err.error.message, alertType.error);
        this.disabled = false;
      },
    });
  }
}
