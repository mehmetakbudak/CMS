import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NzNotificationService } from 'ng-zorro-antd/notification';
import { TokenStorageService } from 'src/app/services/token-storage.service';
import { AuthService } from '../../../services/auth.service';
import { NotificationTypes } from '../../../storage/consts/notification-types';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  form!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private tokenStorage: TokenStorageService,
    private router: Router,
    private notification: NzNotificationService
  ) {}

  ngOnInit() {
    this.form = this.fb.group({
      name: [null, [Validators.required]],
      surname: [null, [Validators.required]],
      emailAddress: [null, [Validators.email, Validators.required]],
      password: [null, [Validators.required]],
      rePassword: [null, [Validators.required]],
      phone: [null, [Validators.required]],
    });
    var tokenInfo = this.tokenStorage.getTokenInfo();
    if (tokenInfo) {
      this.router.navigateByUrl('/');
    }
  }

  save() {
    if (this.form.valid) {
      this.authService.register(this.form.value).subscribe(
        (res) => {
          this.notification.create(
            NotificationTypes.Success,
            'İşlem Başarılı!',
            res.message
          );
          this.form.reset();
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
