import { Component, OnInit } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { AppService } from '../../../services/app.service';
import { Router } from '@angular/router';
import { SharedModule } from '../../../shared.module';

class BlogFilter {
  blogCategoryIds: [] = [];
  title: string = '';
  url: string = '';
  isActive: boolean | null = null;
}

@Component({
  selector: 'app-blog-list',
  templateUrl: './blog-list.component.html',
  styleUrls: ['./blog-list.component.scss'],
  imports: [SharedModule],
})
export class BlogListComponent implements OnInit {
  blogs: any = [];
  // blogCategories: [];
  totalRecords!: number;
  rows = 5;
  first = 0;
  menuItems: any = [];
  gridMenuItems: any = [];
  selectedData: any;
  loading = false;
  title: string = '';
  filterForm!: BlogFilter;
  submitted = false;
  isVisible = false;

  constructor(
    private appService: AppService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private router: Router
  ) {}

  ngOnInit() {
    this.filterForm = new BlogFilter();
    this.getList();
    this.getMenuItems();
    this.getGridMenuItems();

    this.appService.onLangChange().subscribe(() => {
      this.getMenuItems();
      this.getGridMenuItems();
    });
  }

  getMenuItems() {
    this.menuItems = [
      {
        label: this.appService.getTranslate('Add'),
        command: () => {
          this.router.navigate(['/admin/blog/create']);
        },
      },
    ];
  }

  getGridMenuItems() {
    this.gridMenuItems = [
      {
        label: this.appService.getTranslate('Edit'),
        command: () => {
          this.edit(this.selectedData.id);
        },
      },
      {
        label: this.appService.getTranslate('Delete'),
        command: (e: any) => {
          this.delete(e);
        },
      },
    ];
  }

  edit(id: any) {
    this.router.navigate(['/admin/blog/update', id]);
  }

  getList() {
    let filter = '';
    if (this.filterForm.title != '') {
      filter = filter + `contains(tolower(title), '${this.filterForm.title}')`;
    }
    if (this.filterForm.url != '') {
      filter = filter + `contains(tolower(url), '${this.filterForm.url}')`;
    }
    if (filter != '') {
      filter = `$filter=${filter}&`;
    }
    this.appService
      .get(
        `Blog/Get?${filter}$skip=${this.first}&$top=${this.rows}&$count=true&$orderby=updatedDate desc,insertedDate desc`
      )
      .subscribe((res: any) => {
        this.totalRecords = res.total;
        this.blogs = res.data;
      });
  }

  pageChange(e: any) {
    this.first = e.first;
    this.rows = e.rows;
    this.getList();
  }

  menuToggle(menu: any, e: any, data: any) {
    this.gridMenuItems.forEach((menuItem: any) => {
      menuItem.data = data;
    });
    this.selectedData = data;
    menu.toggle(e);
  }

  reset() {
    this.filterForm = new BlogFilter();
    this.getList();
  }

  search() {
    this.getList();
  }

  delete(e: any) {
    this.confirmationService.confirm({
      target: e.target as EventTarget,
      message: this.appService.getTranslate('Alert.Delete'),
      header: this.appService.getTranslate('Delete'),
      icon: 'pi pi-exclamation-triangle',
      acceptLabel: this.appService.getTranslate('Yes'),
      rejectLabel: this.appService.getTranslate('No'),
      acceptIcon: 'none',
      rejectIcon: 'none',
      rejectButtonStyleClass: 'p-button-text',
      accept: () => {
        this.appService
          .delete('Blog/Delete', this.selectedData.id)
          .subscribe((res: any) => {
            this.getList();
            this.messageService.add({
              severity: 'info',
              summary: 'Success',
              detail: 'Deletion was successful',
            });
          });
      },
    });
  }
}
