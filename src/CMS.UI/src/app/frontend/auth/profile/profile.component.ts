import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { AppService } from '../../../services/app.service';
import { SharedModule } from '../../../shared.module';
import { AlertService, alertType } from '../../../services/alert.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
  imports: [SharedModule],
})
export class ProfileComponent implements OnInit {
  form: FormGroup = this.formBuilder.group({
    emailAddress: new FormControl({ value: '', disabled: true }),
    name: ['', [Validators.required]],
    surname: ['', [Validators.required]],
    phone: ['', [Validators.required]],
  });
  provinces: any = [];
  cities: any = [];
  titles: any = [];
  selectProvinceId!: any;
  selectCityName = '';
  submitted = false;

  constructor(
    private formBuilder: FormBuilder,
    private appService: AppService,
    private alertService: AlertService
  ) {}

  ngOnInit() {
    this.getProfile();
  }

  get f(): { [key: string]: AbstractControl } {
    return this.form.controls;
  }

  getProfile() {
    this.appService.get('Auth/GetProfile').subscribe((res: any) => {
      this.form.setValue(res);
    });
  }

  save(e: any) {
    this.submitted = true;
    if (this.form?.invalid) {
      return;
    }
    this.appService.put('Auth/UpdateProfile', this.form.value).subscribe(() => {
      this.alertService.showMessage(
        'Profile updated successfully.',
        alertType.success
      );
    });
  }
}
