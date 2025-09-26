import { environment } from './../../../../environments/environment';
import { Component, OnInit } from '@angular/core';
import { SharedModule } from '../../../shared.module';
import { AppService } from '../../../services/app.service';

@Component({
  selector: 'app-job-list',
  imports: [SharedModule],
  templateUrl: './job-list.component.html',
  styleUrl: './job-list.component.css',
})
export class FE_JobListComponent implements OnInit {
  list: any = [];
  workTypes: any = [];
  jobLocations: any = [];
  tenants: any = [];
  total: number = 0;
  skip: number = 0;
  pageSize: number = 5;
  position = '';
  environment = environment;

  constructor(private appService: AppService) {}

  ngOnInit() {
    this.getList();
    this.getTenants();
    this.getWorkTypes();
    this.getJobLocations();

    this.appService.onLangChange().subscribe(() => {

    });
  }

  getList() {
    let filter = '';
    let conditionals = [];
    if (this.position != '') {
      conditionals.push(`contains(tolower(position), '${this.position}') `);
    }
    let selectedTenants = this.tenants.filter((e: any) => e.checked);
    if (selectedTenants.length > 0) {
      let tenantIds = selectedTenants.map((e: any) => e.id).join(',');
      conditionals.push(`tenantId in(${tenantIds})`);
    }
    let selectedJobLocations = this.jobLocations.filter((e: any) => e.checked);
    if (selectedJobLocations.length > 0) {
      let jobLocationIds = selectedJobLocations.map((e: any) => e.id).join(',');
      conditionals.push(`jobLocationId in(${jobLocationIds})`);
    }
    let selectedWorkTypes = this.workTypes.filter((e: any) => e.checked);
    if (selectedWorkTypes.length > 0) {
      let workTypeIds = selectedWorkTypes.map((e: any) => e.id).join(',');
      conditionals.push(`workType in(${workTypeIds})`);
    }
    if (conditionals.length > 1) {
      filter = '$filter=';
      for (let i = 0; i < conditionals.length; i++) {
        if (i == conditionals.length - 1) {
          filter += conditionals[i];
        } else {
          filter += conditionals[i] + ' and ';
        }
      }
      filter += '&';
    } else if (conditionals.length == 1) {
      filter = `$filter=${conditionals[0]}&`;
    }
    this.appService
      .get(
        `Job/Get?${filter}$skip=${this.skip}&$top=${this.pageSize}&$count=true`
      )
      .subscribe((res: any) => {
        this.list = res.data;
        this.total = res.total;
      });
  }

  getTenants() {
    this.appService.get('Lookup/Tenants').subscribe((res: any) => {
      this.tenants = res.map((e: any) => {
        return {
          ...e,
          checked: false,
        };
      });
    });
  }

  getWorkTypes() {
    this.appService.get('Lookup/WorkTypes').subscribe((res: any) => {
      this.workTypes = res.map((e: any) => {
        return {
          ...e,
          checked: false,
        };
      });
    });
  }

  getJobLocations() {
    this.appService.get('Lookup/JobLocations').subscribe((res: any) => {
      this.jobLocations = res.map((e: any) => {
        return {
          ...e,
          checked: false,
        };
      });
    });
  }

  onPageChange(event: any) {
    this.skip = event.first;
    this.pageSize = event.rows;
    this.getList();
  }

  search() {
    this.getList();
  }

  clearAll() {
    this.position = '';
    this.tenants.forEach((e: any) => {
      e.checked = false;
    });
    this.workTypes.forEach((e: any) => {
      e.checked = false;
    });
    this.jobLocations.forEach((e: any) => {
      e.checked = false;
    });
    this.getList();
  }
}
