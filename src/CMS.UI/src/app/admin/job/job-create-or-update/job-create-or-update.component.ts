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
  selector: 'app-job-create-or-update',
  templateUrl: './job-create-or-update.component.html',
  styleUrl: './job-create-or-update.component.css',
  imports: [SharedModule]
})
export class JobCreateOrUpdateComponent implements OnInit {
  form: FormGroup = this.formBuilder.group({
    id: [0],
    position: ['', Validators.required],
    description: [null],
    tenantId: [null, [Validators.required]],
    workType: [null, [Validators.required]],
    jobLocationId: [null, [Validators.required]],
    isActive: [true, Validators.required],
  });
  id: number = 0;
  tenants: any = [];
  workTypes: any = [];
  jobLocations: any = [];
  submitted = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private appService: AppService,
    private formBuilder: FormBuilder,
    private alertService: AlertService
  ) {}

  ngOnInit() {
    this.getTenants();
    this.getJobLocations();
    this.getWorkTypes();
    this.route.params.subscribe((params) => {
      this.id = params['id'];
      if (this.id > 0) {
        this.appService.get(`Job/GetById/${this.id}`).subscribe((res: any) => {
          this.form.patchValue(res);
        });
      }
    });
  }

  getTenants() {
    this.appService.get('Lookup/Tenants').subscribe((res: any) => {
      this.tenants = res;
    });
  }

  getWorkTypes() {
    this.appService.get('Lookup/WorkTypes').subscribe((res: any) => {
      this.workTypes = res;
    });
  }

  getJobLocations() {
    this.appService.get('Lookup/JobLocations').subscribe((res: any) => {
      this.jobLocations = res;
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
      this.appService.post(`Job/Create`, this.form.value).subscribe((res: any) => {
        this.alertService.showMessage(
          'Addition was successful',
          alertType.success
        );
        setTimeout(() => {
          this.router.navigateByUrl(`/admin/job`);
        }, 1000);
      });
    } else {
      this.appService.put(`Job/Update`, this.form.value).subscribe((res: any) => {
        this.alertService.showMessage(
          'The update was successful',
          alertType.success
        );
        setTimeout(() => {
          this.router.navigateByUrl(`/admin/job`);
        }, 1000);
      });
    }
  }
}
