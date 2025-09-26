import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { MessageService } from 'primeng/api';
import { AppService } from '../../../services/app.service';
import { SharedModule } from '../../../shared.module';

@Component({
    selector: 'app-change-password',
    templateUrl: './change-password.component.html',
    styleUrls: ['./change-password.component.css'],
    imports: [SharedModule]
})
export class ChangePasswordComponent implements OnInit {
  form: FormGroup = this.formBuilder.group({
    oldPassword: ['', [Validators.required]],
    newPassword: ['', [Validators.required]],
    reNewPassword: ['', [Validators.required]],
  });
  submitted = false;

  constructor(
    private formBuilder: FormBuilder,
    private appService: AppService,
    private messageService: MessageService
  ) {}

  ngOnInit() {
    this.form;
  }

  get f(): { [key: string]: AbstractControl } {
    return this.form.controls;
  }

  save() {
    this.submitted = true;
    if (this.form?.invalid) {
      return;
    }
    if (this.form.value.newPassword !== this.form.value.reNewPassword) {
      this.messageService.add({
        severity: 'error',
        summary: 'Success',
        detail: "Password fields don't matched.",
      });
      return;
    }
    this.appService.post('Auth/ChangePassword', this.form.value).subscribe(
      (res) => {
        this.messageService.add({
          severity: 'info',
          summary: 'Success',
          detail: 'Password has been updated successfully.',
        });
        this.form.reset();
      },
      (error) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: error,
        });
      }
    );
  }
}
