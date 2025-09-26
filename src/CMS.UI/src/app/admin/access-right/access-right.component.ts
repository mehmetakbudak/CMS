import { FormBuilder, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { AppService } from '../../services/app.service';
import {
  AbstractControl
} from '@angular/forms';
import { ConfirmationService } from 'primeng/api';
import { AlertService, alertType } from '../../services/alert.service';
import { SharedModule } from '../../shared.module';

@Component({
  selector: 'app-access-right',
  templateUrl: './access-right.component.html',
  styleUrl: './access-right.component.css',
  imports: [SharedModule],
})
export class AccessRightComponent implements OnInit {
  formAccessRightCategory = this.formBuilder.group({
    id: [0, []],
    name: [null, Validators.required],
    displayOrder: [null, Validators.required],
    isActive: [true, Validators.required],
  });
  formAccessRight = this.formBuilder.group({
    id: [0, []],
    accessRightCategoryId: [0, []],
    name: [null, Validators.required],
    displayOrder: [null, Validators.required],
    isActive: [true, Validators.required],
  });
  formAccessRightEndpoint = this.formBuilder.group({
    id: [0, []],
    accessRightId: [0, []],
    endpoint: [null, Validators.required],
    method: [null, Validators.required],
  });
  loading = false;
  //#region Access Right Category
  accessRightCategories: any = [];
  menuItemsCategory: any = [];
  titleAccessRightCategory = '';
  showAccessRightCategory = false;
  submittedAccessRightCategory = false;
  selectedCategory: any;
  selectedAccessRightCategory: any;
  //#endregion
  //#region Access Right
  accessRights: any = [];
  menuItemsAccessRight: any = [];
  titleAccessRight = '';
  selectedAccessRight: any;
  showAccessRight = false;
  submittedAccessRight = false;
  //#endregion
  //#region Access Right Endpoint
  menuItemsAccessRightEndpoint: any = [];
  showAccessRightEndpoints = false;
  accessRightEndpoints: any = [];
  showAccessRightEndpointForm = false;
  titleAccessRightEndpointForm = '';
  submittedAccessRightEndpoint = false;
  methodTypes: any = [];
  selectedAccessRightEndpoint: any;
  //#endregion

  constructor(
    private appService: AppService,
    private formBuilder: FormBuilder,
    private alertService: AlertService,
    private confirmationService: ConfirmationService
  ) {}

  ngOnInit() {
    this.getAccessRightCategories();
    this.menuItemsCategory = [
      {
        label: 'Edit',
        command: () => {
          this.getAccessRightCategory(this.selectedCategory.id);
          this.showAccessRightCategory = true;
          this.titleAccessRightCategory = 'Edit Access Right Category';
        },
      },
      {
        label: 'Delete',
        command: (e: any) => {
          this.confirmationService.confirm({
            target: e.target as EventTarget,
            message: 'Are you sure that you want to delete?',
            header: 'Delete',
            icon: 'pi pi-exclamation-triangle',
            acceptIcon: 'none',
            rejectIcon: 'none',
            rejectButtonStyleClass: 'p-button-text',
            accept: () => {
              this.appService
                .delete('AccessRightCategory/Delete', this.selectedCategory.id)
                .subscribe((res: any) => {
                  this.getAccessRightCategories();
                  this.alertService.showMessage(
                    'Deletion was successful',
                    alertType.success
                  );
                });
            },
          });
        },
      },
    ];
    this.menuItemsAccessRight = [
      {
        label: 'Endpoints',
        command: () => {
          this.showAccessRightEndpoints = true;
          this.getAccessRightEndpoints();
        },
      },
      {
        label: 'Edit',
        command: () => {
          this.getAccessRight(this.selectedAccessRight.id);
          this.showAccessRight = true;
          this.titleAccessRight = 'Edit Access Right';
        },
      },
      {
        label: 'Delete',
        command: (e: any) => {
          this.confirmationService.confirm({
            target: e.target as EventTarget,
            message: 'Are you sure that you want to delete?',
            header: 'Delete',
            icon: 'pi pi-exclamation-triangle',
            acceptIcon: 'none',
            rejectIcon: 'none',
            rejectButtonStyleClass: 'p-button-text',
            accept: () => {
              this.appService
                .delete('AccessRight/Delete', this.selectedAccessRight.id)
                .subscribe((res: any) => {
                  this.getAccessRights();
                  this.alertService.showMessage(
                    'Deletion was successful',
                    alertType.success
                  );
                });
            },
          });
        },
      },
    ];
    this.menuItemsAccessRightEndpoint = [
      {
        label: 'Edit',
        command: () => {
          this.showAccessRightEndpointForm = true;
          this.titleAccessRightEndpointForm = 'Edit Access Right Endpoint';
          this.getAccessRightEndpoint(this.selectedAccessRightEndpoint.id);
          this.getMethodTypes();
        },
      },
      {
        label: 'Delete',
        command: (e: any) => {
          this.confirmationService.confirm({
            target: e.target as EventTarget,
            message: 'Are you sure that you want to delete?',
            header: 'Delete',
            icon: 'pi pi-exclamation-triangle',
            acceptIcon: 'none',
            rejectIcon: 'none',
            rejectButtonStyleClass: 'p-button-text',
            accept: () => {
              this.appService
                .delete('AccessRightEndpoint/Delete', this.selectedAccessRightEndpoint.id)
                .subscribe((res: any) => {
                  this.getAccessRightEndpoints();
                  this.alertService.showMessage(
                    'Deletion was successful',
                    alertType.success
                  );
                });
            },
          });
        },
      },
    ];
  }

  getAccessRightCategories() {
    this.loading = true;
    this.appService.get(`AccessRightCategory/Get`).subscribe((res: any) => {
      this.loading = false;
      this.accessRightCategories = res.data;
    });
  }

  getAccessRights() {
    this.appService
      .get(
        `AccessRight/Get?$filter=accessRightCategoryId eq ${this.selectedAccessRightCategory.id}&$orderby=displayOrder`
      )
      .subscribe((res: any) => {
        this.accessRights = res.data;
      });
  }

  getAccessRightEndpoints() {
    this.appService
      .get(
        `AccessRightEndpoint/Get?$filter=accessRightId eq ${this.selectedAccessRight.id}&$orderby=id desc`
      )
      .subscribe((res: any) => {
        this.accessRightEndpoints = res.data;
      });
  }

  getMethodTypes() {
    this.appService.get(`Lookup/MethodTypes`).subscribe((res: any) => {
      this.methodTypes = res;
    });
  }

  menuToggleCategory(menu: any, e: any, data: any) {
    this.menuItemsCategory.forEach((menuItem: any) => {
      menuItem.data = data;
    });
    this.selectedCategory = data;
    menu.toggle(e);
  }

  menuToggleAccessRight(menu: any, e: any, data: any) {
    this.menuItemsAccessRight.forEach((menuItem: any) => {
      menuItem.data = data;
    });
    this.selectedAccessRight = data;
    menu.toggle(e);
  }

  menuToggleAccessRightEndpoint(menu: any, e: any, data: any) {
    this.menuItemsAccessRightEndpoint.forEach((menuItem: any) => {
      menuItem.data = data;
    });
    this.selectedAccessRightEndpoint = data;
    menu.toggle(e);
  }

  onRowExpand(e: any) {
    this.selectedAccessRightCategory = e.data;
    this.getAccessRights();
  }

  get fAccessRightCategory(): { [key: string]: AbstractControl } {
    return this.formAccessRightCategory.controls;
  }

  get fAccessRight(): { [key: string]: AbstractControl } {
    return this.formAccessRight.controls;
  }

  get fAccessRightEndpoint(): { [key: string]: AbstractControl } {
    return this.formAccessRightEndpoint.controls;
  }

  getAccessRightCategory(id: number) {
    this.appService
      .get(`AccessRightCategory/GetById/${id}`)
      .subscribe((res: any) => {
        this.formAccessRightCategory.patchValue(res);
      });
  }

  getAccessRight(id: number) {
    this.appService.get(`AccessRight/GetById/${id}`).subscribe((res: any) => {
      this.formAccessRight.patchValue(res);
    });
  }

  getAccessRightEndpoint(id: number) {
    this.appService
      .get(`AccessRightEndpoint/GetById/${id}`)
      .subscribe((res: any) => {
        this.formAccessRightEndpoint.patchValue(res);
      });
  }

  addAccessRightCategory() {
    this.formAccessRightCategory.reset();
    this.formAccessRightCategory.patchValue({
      id: 0,
      isActive: true,
    });
    this.submittedAccessRightCategory = false;
    this.showAccessRightCategory = true;
    this.titleAccessRightCategory = 'Add Access Right Category';
  }

  saveAccessRightCategory() {
    this.submittedAccessRightCategory = true;
    if (this.formAccessRightCategory?.invalid) {
      return;
    }
    if (this.formAccessRightCategory.value.id == 0) {
      this.appService
        .post('AccessRightCategory/Create', this.formAccessRightCategory.value)
        .subscribe((res: any) => {
          this.getAccessRightCategories();
          this.alertService.showMessage(
            'Addition was successful',
            alertType.success
          );
          this.showAccessRightCategory = false;
        });
    } else {
      this.appService
        .put('AccessRightCategory/Update', this.formAccessRightCategory.value)
        .subscribe((res: any) => {
          this.getAccessRightCategories();
          this.alertService.showMessage(
            'The update was successful',
            alertType.success
          );
          this.showAccessRightCategory = false;
        });
    }
  }

  addAccessRight() {
    this.formAccessRight.reset();
    this.formAccessRight.patchValue({
      id: 0,
      accessRightCategoryId: this.selectedAccessRightCategory.id,
      isActive: true,
    });
    this.submittedAccessRight = false;
    this.showAccessRight = true;
    this.titleAccessRight = 'Add Access Right';
  }

  saveAccessRight() {
    this.submittedAccessRight = true;
    if (this.formAccessRight?.invalid) {
      return;
    }
    if (this.formAccessRight.value.id == 0) {
      this.appService
        .post('AccessRight/Create', this.formAccessRight.value)
        .subscribe((res: any) => {
          this.getAccessRights();
          this.alertService.showMessage(
            'Addition was successful',
            alertType.success
          );
          this.showAccessRight = false;
        });
    } else {
      this.appService
        .put('AccessRight/Update', this.formAccessRight.value)
        .subscribe((res: any) => {
          this.getAccessRights();
          this.alertService.showMessage(
            'The update was successful',
            alertType.success
          );
          this.showAccessRight = false;
        });
    }
  }

  addAccessRightEndpoint() {
    this.formAccessRightEndpoint.reset();
    this.formAccessRightEndpoint.patchValue({
      id: 0,
      accessRightId: this.selectedAccessRight.id,
    });
    this.submittedAccessRightEndpoint = false;
    this.showAccessRightEndpointForm = true;
    this.titleAccessRightEndpointForm = 'Add Access Right Endpoint';
    this.getMethodTypes();
  }

  saveAccessRightEndpoint() {
    this.submittedAccessRightEndpoint = true;
    if (this.formAccessRightEndpoint?.invalid) {
      return;
    }
    if (this.formAccessRightEndpoint.value.id == 0) {
      this.appService
        .post('AccessRightEndpoint/Create', this.formAccessRightEndpoint.value)
        .subscribe((res: any) => {
          this.getAccessRightEndpoints();
          this.alertService.showMessage(
            'Addition was successful',
            alertType.success
          );
          this.showAccessRightEndpointForm = false;
        });
    } else {
      this.appService
        .put('AccessRightEndpoint/Update', this.formAccessRightEndpoint.value)
        .subscribe((res: any) => {
          this.getAccessRightEndpoints();
          this.alertService.showMessage(
            'The update was successful',
            alertType.success
          );
          this.showAccessRightEndpointForm = false;
        });
    }
  }
}
