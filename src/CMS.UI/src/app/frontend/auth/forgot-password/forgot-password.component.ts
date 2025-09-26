import { Component, OnInit } from '@angular/core';
import { SharedModule } from '../../../shared.module';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { AppService } from '../../../services/app.service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-forgot-password',
  imports: [SharedModule],
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.css',
})
export class ForgotPasswordComponent implements OnInit {
  form: FormGroup = this.formBuilder.group({
    emailAddress: ['', [Validators.required, Validators.email]],
  });
  submitted = false;
  disabled = false;
  loading = false;

  constructor(
    private formBuilder: FormBuilder,
    private messageService: MessageService,
    private appService: AppService
  ) {}

  ngOnInit(): void {}

  get f(): { [key: string]: AbstractControl } {
    return this.form.controls;
  }

  onSubmit() {
    this.submitted = true;
    if (this.form?.invalid) {
      return;
    }
    this.disabled = true;
    this.appService
      .post('Auth/ForgotPassword', this.form.value)
      .subscribe((res: any) => {
        this.messageService.add({
          severity: 'info',
          summary: 'Success',
          detail: res.message,
        });
        this.form.reset();
        this.submitted = false;
        this.disabled = false;
      });
  }
}
