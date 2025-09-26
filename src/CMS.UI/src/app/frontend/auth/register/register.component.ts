import { Component } from '@angular/core';
import { Router } from '@angular/router';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { AppService } from '../../../services/app.service';
import { AlertService, alertType } from '../../../services/alert.service';
import { SharedModule } from '../../../shared.module';

@Component({
  selector: 'app-register',
  imports: [SharedModule],
  templateUrl: './register.component.html',
  standalone: true,
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  form: FormGroup = this.formBuilder.group({
    name: ['', [Validators.required]],
    surname: ['', [Validators.required]],
    emailAddress: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
    rePassword: ['', [Validators.required]],
    phone: ['', [Validators.required]],
  });
  submitted = false;
  disabled = false;
  loading = false;
  returnUrl!: string;

  constructor(
    private router: Router,
    private alertService: AlertService,
    private formBuilder: FormBuilder,
    private appService: AppService
  ) {}

  get f(): { [key: string]: AbstractControl } {
    return this.form.controls;
  }

  onSubmit() {
    this.submitted = true;
    if (this.form?.invalid) {
      return;
    }
    this.disabled = true;
    this.appService.post('Auth/Register', this.form.value).subscribe({
      next: (res: any) => {
        this.alertService.showMessage(
          'User registered successfully',
          alertType.success
        );
        setTimeout(() => {
          this.router.navigateByUrl('/login');
        }, 1000);
      },
      error: (err: any) => {
        this.alertService.showMessage(err.error.message, alertType.error);
        this.disabled = false;
      },
    });
  }
}
