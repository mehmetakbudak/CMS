import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ConfirmationService, MessageService } from 'primeng/api';
import { AppService } from '../../services/app.service';
import { SharedModule } from '../../shared.module';

class PageFilter {
  name!: string;
  url!: string;
  isActive!: boolean;
}

@Component({
  selector: 'app-page',
  templateUrl: './page.component.html',
  styleUrls: ['./page.component.css'],
  standalone: true,
  imports: [SharedModule],
})
export class PageComponent implements OnInit {
  form: FormGroup = this.formBuilder.group({
    id: [0, []],
    menuId: [null],
    menuItemId: [null],
    menuItem: [],
    name: ['', Validators.required],
    title: ['', [Validators.required]],
    isActive: [true, Validators.required],
    published: [true, Validators.required],
  });
  totalRecords!: number;
  rows = 5;
  first = 0;
  menuItems: any = [];
  gridMenuItems: any = [];
  loading = false;
  selectedData: any;
  filterForm!: PageFilter;
  submitted = false;
  pages: any = [];
  isVisible = false;
  title = '';
  menus: any = [];
  selectedMenuItems: any = [];
  editLoading = false;
  content = '';

  constructor(
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private appService: AppService,
    private formBuilder: FormBuilder
  ) {}

  ngOnInit() {
    this.filterForm = new PageFilter();
    this.getPages();
    this.menuItems = [
      {
        label: 'Add',
        command: () => {
          this.add();
        },
      },
    ];
    this.gridMenuItems = [
      {
        label: 'Edit',
        command: () => {
          this.edit(this.selectedData.id);
        },
      },
      {
        label: 'Delete',
        command: (e: any) => {
          this.delete(e);
        },
      },
    ];
  }

  getPages() {
    this.appService.get(`Page/Get?$count=true&$orderBy=id desc`).subscribe((res: any) => {
      this.pages = res.data;
      this.totalRecords = res.total;
    });
  }

  getMenus() {
    this.appService.get(`Menu/Get?$filter=type eq 3`).subscribe((res: any) => {
      this.menus = res.data;
    });
  }

  onMenuChange(e: any) {
    this.selectedMenuItems = [];
    if (e.value) {
      this.appService
        .get(`MenuItem/GetMenuItemsByMenuId/${e.value}`)
        .subscribe((res: any) => {
          this.selectedMenuItems = res;
        });
    } else {
      this.form.patchValue({ menuItemId: null, menuItem: null });
    }
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

  get f(): { [key: string]: AbstractControl } {
    return this.form.controls;
  }

  pageChange(e: any) {
    this.first = e.first;
    this.rows = e.rows;
    this.getPages();
  }

  menuToggle(menu: any, e: any, data: any) {
    this.gridMenuItems.forEach((menuItem: any) => {
      menuItem.data = data;
    });
    this.selectedData = data;
    menu.toggle(e);
  }

  reset() {
    this.filterForm = new PageFilter();
    this.getPages();
  }

  search() {
    this.getPages();
  }

  add() {
    this.getMenus();
    this.form.reset();
    this.isVisible = true;
    this.content = '';
    this.selectedMenuItems = [];
    this.form.patchValue({ id: 0, isActive: true, published: true });
    this.title = 'Add Page';
  }

  edit(id: any) {
    this.editLoading = true;
    this.content = '';
    this.getMenus();
    this.selectedMenuItems = [];
    this.form.reset();
    this.appService.get(`Page/GetById/${id}`).subscribe((res: any) => {
      if (res.menuItemId) {
        this.appService
          .get(`MenuItem/GetMenuItemsByMenuId/${res.menuId}`)
          .subscribe((menuItems: any) => {
            this.selectedMenuItems = menuItems;
            var menuItemValue = this.findNodeByData(
              this.selectedMenuItems,
              res.menuItemId
            );
            this.form.patchValue({
              ...res,
              menuItem: menuItemValue,
            });
            this.editLoading = false;
          });
      } else {
        this.editLoading = false;
        this.form.patchValue(res);
      }
      this.content = res.content;
      this.isVisible = true;
      this.title = 'Edit Page';
    });
  }

  delete(event: any) {
    this.confirmationService.confirm({
      target: event.target as EventTarget,
      message: 'Are you sure that you want to delete?',
      header: 'Delete',
      icon: 'pi pi-exclamation-triangle',
      acceptIcon: 'none',
      rejectIcon: 'none',
      rejectButtonStyleClass: 'p-button-text',
      accept: () => {
        this.appService
          .delete(`Page/Delete`, this.selectedData.id)
          .subscribe((res: any) => {
            this.getPages();
            this.messageService.add({
              severity: 'info',
              summary: 'Success',
              detail: 'Deletion was successful',
            });
          });
      },
    });
  }

  save() {
    this.submitted = true;
    if (this.form?.invalid) {
      return;
    }
    var value = this.form.value;
    var data = {
      id: value.id,
      menuItemId: value.menuItem ? value.menuItem.id : null,
      name: value.name,
      title: value.title,
      content: this.content,
      isActive: value.isActive,
      published: value.published,
    };
    if (value.id == 0) {
      this.appService.post('Page/Create', data).subscribe((res: any) => {
        this.getPages();
        this.messageService.add({
          severity: 'info',
          summary: 'Success',
          detail: 'Addition was successful',
        });
        this.isVisible = false;
      });
    } else {
      this.appService.put('Page/Update', data).subscribe((res: any) => {
        this.getPages();
        this.messageService.add({
          severity: 'info',
          summary: 'Success',
          detail: 'The update was successful',
        });
        this.isVisible = false;
      });
    }
  }
}
