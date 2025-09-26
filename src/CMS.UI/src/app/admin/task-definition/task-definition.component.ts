import { Component, OnInit } from '@angular/core';
import { AppService } from '../../services/app.service';
import { SharedModule } from '../../shared.module';
@Component({
  selector: 'app-task-definition',
  templateUrl: './task-definition.component.html',
  styleUrl: './task-definition.component.css',
  imports: [SharedModule],
})
export class TaskDefinitionComponent implements OnInit {
  taskCategories: any = [];
  taskStatuses: any = [];
  menuItems: any = [];
  gridMenuItems: any = [];
  selectedData: any;
  loading = false;

  constructor(private appService: AppService) {}

  ngOnInit() {
    this.getTaskCategories();
    this.menuItems = [
      {
        label: 'Add',
        command: () => {},
      },
    ];
    this.gridMenuItems = [
      {
        label: 'Edit',
        command: () => {},
      },
      {
        label: 'Delete',
        command: (e: any) => {},
      },
    ];
  }

  getTaskCategories() {
    this.loading = true;
    this.appService.get(`TaskCategory/Get`).subscribe((res: any) => {
      this.loading = false;
      this.taskCategories = res.data;
    });
  }

  menuToggle(menu: any, e: any, data: any) {
    this.gridMenuItems.forEach((menuItem: any) => {
      menuItem.data = data;
    });
    this.selectedData = data;
    menu.toggle(e);
  }

  onRowExpand(e: any) {
    this.appService
      .get(
        `TaskStatus/Get?$filter=taskCategoryId eq ${e.data.id}&$orderby=displayOrder`
      )
      .subscribe((res: any) => {
        this.taskStatuses = res.data;
      });
  }

  search() {}

  reset() {}
}
