import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { AuthService } from '../../../services/auth.service';
import { TokenStorageService } from '../../../services/token-storage.service';
import { NotificationTypes } from '../../../storage/consts/notification-types';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  form!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private tokenStorage: TokenStorageService,
    private notification: NzNotificationService
  ) {}

  ngOnInit() {
    this.form = this.fb.group({
      emailAddress: [null, [Validators.email, Validators.required]],
      password: [null, [Validators.required]],
    });
    var tokenInfo = this.tokenStorage.getTokenInfo();
    if(tokenInfo){
      this.router.navigateByUrl("/");
    }
  }

  login() {
    if (this.form.valid) {
      this.authService.login(this.form.value).subscribe(
        (data) => {
          this.tokenStorage.saveTokenInfo(JSON.stringify(data));
          this.notification.create(
            NotificationTypes.Success,
            'Hoşgeldiniz!',
            data.fullName
          );

          setTimeout(() => {
            window.location.href = '/';
          }, 1000);
        },
        (err) => {
          this.notification.create(
            NotificationTypes.Error,
            'İşlem Başarısız!',
            err.error.message
          );
        }
      );
    } else {
      Object.values(this.form.controls).forEach((control) => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }
}
