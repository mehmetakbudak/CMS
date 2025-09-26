import { Component, OnInit } from '@angular/core';
import { AppService } from '../../../services/app.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Router } from '@angular/router';
import { SharedModule } from '../../../shared.module';

class TaskFilter {
  taskCategoryId: number | null = null;
  taskStatusIds: [] = [];
  title: string = '';
}

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css'],
  imports: [SharedModule],
})
export class TaskListComponent implements OnInit {
  tasks: any = [];
  filterForm!: TaskFilter;
  totalRecords!: number;
  rows = 5;
  first = 0;
  loading = false;
  menuItems: any = [];
  gridMenuItems: any = [];
  selectedData: any;
  taskCategories: any = [];
  taskStatuses: any = [];
  title = '';
  taskCategoryHeader = '';
  expandedRows = {};
  selectedTab = 0;

  constructor(
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private appService: AppService,
    private router: Router
  ) {}

  ngOnInit() {
    this.filterForm = new TaskFilter();
    this.getTasks();
    this.getTaskCategories();
    this.menuItems = [
      {
        label: 'Add',
        command: () => {
          this.router.navigateByUrl(`/admin/task/create`);
        },
      },
      {
        label: 'Expand All',
        command: () => {
          this.expandAll();
        },
      },
      {
        label: 'Collapse All',
        command: () => {
          this.collapseAll();
        },
      },
    ];
    this.gridMenuItems = [
      {
        label: 'Edit',
        command: (e: any) => {
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

  edit(id: any) {
    this.router.navigateByUrl(`/admin/task/update/${id}`);
  }

  getTaskCategories() {
    this.appService.get('Lookup/TaskCategories').subscribe((res: any) => {
      this.taskCategories = res;
    });
  }

  onSelectTaskCategory(e: any) {
    if (e.value) {
      this.appService.get(`Lookup/TaskStatuses/${e.value}`).subscribe((res: any) => {
        this.taskStatuses = res;
      });
    }
  }

  getTasks() {
    this.loading = true;
    let filter = '';
    let conditionals = [];
    conditionals.push(`isCompleted eq ${this.selectedTab == 1 ? true : false}`);
    if (this.filterForm.taskCategoryId != null) {
      conditionals.push(`taskCategoryId eq ${this.filterForm.taskCategoryId}`);
    }
    if (this.filterForm.taskStatusIds.length > 0) {
      conditionals.push(
        `taskStatusId in(${this.filterForm.taskStatusIds.join(',')})`
      );
    }
    if (this.filterForm.title != '') {
      conditionals.push(`contains(tolower(title), '${this.filterForm.title}')`);
    }
    if (this.filterForm.title != '') {
      conditionals.push(`contains(tolower(title), '${this.filterForm.title}')`);
    }
    // if (this.filterForm.isActive != null) {
    //   conditionals.push(`isActive eq ${this.filterForm.isActive}`);
    // }
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
        `Task/Get?${filter}$skip=${this.first}&$top=${this.rows}&$count=true&$orderby=updatedDate desc,insertedDate desc`
      )
      .subscribe((res: any) => {
        this.tasks = res.data;
        this.totalRecords = res.total;
        this.loading = false;
      });
  }

  pageChange(e: any) {
    this.first = e.first;
    this.rows = e.rows;
    this.getTasks();
  }

  menuToggle(menu: any, e: any, data: any) {
    this.gridMenuItems.forEach((menuItem: any) => {
      menuItem.data = data;
    });
    this.selectedData = data;
    menu.toggle(e);
  }

  reset() {
    this.filterForm = new TaskFilter();
    this.getTasks();
  }

  search() {
    this.getTasks();
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
          .delete(`Task`, this.selectedData.id)
          .subscribe((res: any) => {
            this.getTasks();
            this.messageService.add({
              severity: 'info',
              summary: 'Success',
              detail: 'Deletion was successful',
            });
          });
      },
    });
  }

  expandAll() {
    this.expandedRows = this.tasks.reduce(
      (acc: any, p: any) => (acc[p.id] = true) && acc,
      {}
    );
  }

  collapseAll() {
    this.expandedRows = {};
  }

  onTabChange(e: any) {
    this.rows = 5;
    this.first = 0;
    this.selectedTab = e;
    this.getTasks();
  }
}
