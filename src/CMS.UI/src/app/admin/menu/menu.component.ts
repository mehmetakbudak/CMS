import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ConfirmationService } from 'primeng/api';
import { AppService } from '../../services/app.service';
import { SharedModule } from '../../shared.module';
import { AlertService, alertType } from '../../services/alert.service';
import { MenuType } from '../../models/enums';
class MenuFilter {
  name!: string;
  url!: string;
  isActive: boolean | null = null;
}

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css'],
  imports: [SharedModule],
})
export class MenuComponent implements OnInit {
  formMenu: FormGroup = this.formBuilder.group({
    id: [],
    name: ['', Validators.required],
    type: [null, Validators.required],
    isActive: [true, Validators.required],
  });
  formMenuItem: FormGroup = this.formBuilder.group({
    id: [],
    title: ['', Validators.required],
    url: [''],
    menuId: [null],
    parentId: [null],
    parentValue: [{}],
    displayOrder: [0, Validators.required],
    isActive: [true, Validators.required],
    accessRightIds: [[]],
  });
  totalRecords!: number;
  rows = 5;
  first = 0;
  menuItems: any = [];
  gridMenuItems: any = [];
  loading = false;
  selectedData: any;
  filterForm!: MenuFilter;
  submittedMenu = false;
  menus: any = [];
  visibleMenuModal = false;
  title = '';
  menuTypes: any = [];
  selectedMenuData: any = null;
  selectedMenuItems: any = [];
  loadingMenuItems = false;
  menuItemTitle = '';
  visibleMenuItemModal = false;
  treeMenuItems: any = [];
  selectedMenuItemData: any = null;
  submittedMenuItem = false;
  accessRights: any = [];
  menuType = MenuType;

  constructor(
    private confirmationService: ConfirmationService,
    private appService: AppService,
    private formBuilder: FormBuilder,
    private alertService: AlertService
  ) {}

  ngOnInit() {
    this.filterForm = new MenuFilter();
    this.getMenus();
    this.getMenuTypes();
    this.menuItems = [
      {
        label: 'Add',
        command: () => {
          this.addMenu();
        },
      },
    ];
    this.gridMenuItems = [
      {
        label: 'Edit',
        command: () => {
          this.editMenu(this.selectedData.id);
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
                .delete(`Menu/Delete`, this.selectedData.id)
                .subscribe((res: any) => {
                  this.getMenus();
                  this.alertService.showMessage(
                    'The delete was successful',
                    alertType.success
                  );
                });
            },
          });
        },
      },
    ];
    this.treeMenuItems = [
      {
        label: 'Edit',
        command: () => {
          if (this.selectedMenuData.type == this.menuType.Admin) {
            this.getAccessRights();
          }
          this.appService
            .get(`MenuItem/GetById/${this.selectedMenuItemData.id}`)
            .subscribe((res: any) => {
              var parentValue = this.findNodeByData(
                this.selectedMenuItems,
                res.parentId
              );
              this.formMenuItem.patchValue({
                ...res,
                parentValue: parentValue,
              });
              this.visibleMenuItemModal = true;
              this.menuItemTitle = 'Edit Item Menu';
            });
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
                .delete(`MenuItem/Delete`, this.selectedMenuItemData.id)
                .subscribe((res: any) => {
                  this.getMenus();
                  this.alertService.showMessage(
                    'The delete was successful',
                    alertType.success
                  );
                });
            },
          });
        },
      },
    ];
  }

  findNodeByData(items: any, id: number): any {
    for (let item of items) {
      if (item.id === id) {
        return item;
      }
      if (item.children) {
        const data = this.findNodeByData(item.children, id);
        if (data) {
          return data;
        }
      }
    }
    return null;
  }

  getMenus() {
    let filter = '';
    this.appService
      .get(
        `Menu/Get?${filter}$skip=${this.first}&$top=${this.rows}&$count=true&orderBy=id desc`
      )
      .subscribe((res: any) => {
        this.menus = res.data;
        this.totalRecords = res.total;
      });
  }

  getAccessRights() {
    this.appService
      .get('AccessRightCategory/GetWithAccessRights')
      .subscribe((res: any) => {
        this.accessRights = res;
      });
  }

  onRowExpand(event: any) {
    this.selectedMenuData = event.data;
    this.getMenuItems();
  }

  getMenuItems() {
    this.selectedMenuItems = [];
    this.loadingMenuItems = true;
    this.appService
      .get(`MenuItem/GetMenuItemsByMenuId/${this.selectedMenuData.id}`)
      .subscribe((res: any) => {
        this.selectedMenuItems = res;
        this.loadingMenuItems = false;
      });
  }

  getMenuTypes() {
    this.appService.get('Lookup/MenuTypes').subscribe((res: any) => {
      this.menuTypes = res;
    });
  }

  getMenuTypeName(id: number) {
    let menuType = this.menuTypes.find((x: any) => x.id == id);
    return menuType ? menuType.name : '';
  }

  get fMenu(): { [key: string]: AbstractControl } {
    return this.formMenu.controls;
  }

  pageChange(e: any) {
    this.first = e.first;
    this.rows = e.rows;
    this.getMenus();
  }

  menuToggle(menu: any, e: any, data: any) {
    this.gridMenuItems.forEach((menuItem: any) => {
      menuItem.data = data;
    });
    this.selectedData = data;
    menu.toggle(e);
  }

  reset() {
    this.filterForm = new MenuFilter();
    this.getMenus();
  }

  search() {
    this.getMenus();
  }

  addMenu() {
    this.formMenu.reset();
    this.formMenu.patchValue({ id: 0, isActive: true });
    this.visibleMenuModal = true;
    this.title = 'Add Menu';
  }

  editMenu(id: number) {
    this.appService.get(`Menu/GetById/${id}`).subscribe((res: any) => {
      this.formMenu.patchValue(res);
      this.visibleMenuModal = true;
      this.title = 'Edit Menu';
    });
  }

  saveMenu() {
    this.submittedMenu = true;
    if (this.formMenu?.invalid) {
      return;
    }
    if (this.formMenu.value.id == 0) {
      this.appService
        .post('Menu/Create', this.formMenu.value)
        .subscribe((res: any) => {
          this.getMenus();
          this.visibleMenuModal = false;
          this.alertService.showMessage(
            'Addition was successful',
            alertType.success
          );
        });
    } else {
      this.appService
        .put('Menu/Update', this.formMenu.value)
        .subscribe((res: any) => {
          this.getMenus();
          this.visibleMenuModal = false;
          this.alertService.showMessage(
            'The update was successful',
            alertType.success
          );
        });
    }
  }

  addMenuItem() {
    this.formMenuItem.reset();
    this.formMenuItem.patchValue({ id: 0, isActive: true });
    this.visibleMenuItemModal = true;
    this.menuItemTitle = 'Add Menu Item';
  }

  get fMenuItem(): { [key: string]: AbstractControl } {
    return this.formMenuItem.controls;
  }

  menuItemToggle(menu: any, e: any, data: any) {
    this.treeMenuItems.forEach((menuItem: any) => {
      menuItem.data = data;
    });
    this.selectedMenuItemData = data;
    menu.toggle(e);
  }

  saveMenuItem() {
    this.submittedMenuItem = true;
    var value = this.formMenuItem.value;
    console.log(value);
    var data = {
      id: value.id,
      title: value.title,
      menuId: this.selectedMenuData.id,
      isActive: value.isActive,
      displayOrder: value.displayOrder,
      parentId: value.parentValue?.id,
      accessRightIds: value.accessRightIds,
      url: value.url,
    };
    if (this.formMenuItem?.invalid) {
      return;
    }
    if (data.id == 0) {
      this.appService.post('MenuItem/Create', data).subscribe((res: any) => {
        this.getMenuItems();
        this.visibleMenuItemModal = false;
        this.alertService.showMessage(
          'Addition was successful',
          alertType.success
        );
      });
    } else {
      this.appService.put('MenuItem/Update', data).subscribe((res: any) => {
        this.getMenuItems();
        this.visibleMenuItemModal = false;
        this.alertService.showMessage(
          'The update was successful',
          alertType.success
        );
      });
    }
  }
}
