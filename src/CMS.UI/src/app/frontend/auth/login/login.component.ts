import { AlertService, alertType } from './../../../services/alert.service';
import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { UserType } from '../../../models/enums';
import { SharedModule } from '../../../shared.module';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  imports: [SharedModule],
})
export class LoginComponent implements OnInit {
  form: FormGroup = this.formBuilder.group({
    emailAddress: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
  });
  submitted = false;
  disabled = false;
  loading = false;
  returnUrl!: string;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private alertService: AlertService,
    private formBuilder: FormBuilder,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.getReturnUrl();
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
    this.authService.login(this.form.value).subscribe(
      (res: any) => {
        this.authService.setUserInfo(res);
        this.authService.setIsAuthenticate(true);
        this.alertService.showMessage(
          'Welcome ' + res.fullName,
          alertType.success
        );
        setTimeout(() => {
          this.getReturnUrl();
          this.router.navigateByUrl(this.returnUrl);
        }, 1000);
      },
      (err: any) => {
        this.loading = false;
        this.disabled = false;
        this.alertService.showMessage(err.error.message, alertType.error);
      }
    );
  }

  public clearForm(): void {
    this.form.reset();
  }

  getReturnUrl() {
    var returnUrl = this.route.snapshot.queryParams['returnUrl'];
    if (returnUrl) {
      this.returnUrl = returnUrl;
    } else {
      var user = this.authService.getUserInfo();
      if (user && user.userType != UserType.Member) {
        this.returnUrl = '/admin';
      } else {
        this.returnUrl = '/';
      }
    }
  }
}
