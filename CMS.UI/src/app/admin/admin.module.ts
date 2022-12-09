import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { AdminRoutes } from './admin.routing';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzPopconfirmModule } from 'ng-zorro-antd/popconfirm';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzPaginationModule } from 'ng-zorro-antd/pagination';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzNotificationModule } from 'ng-zorro-antd/notification';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzPageHeaderModule } from 'ng-zorro-antd/page-header';
import { NzSpaceModule } from 'ng-zorro-antd/space';

import { DashboardComponent } from './dashboard/dashboard.component';
import { BlogListComponent } from './blogs/blog-list/blog-list.component';
import { AdminLayoutComponent } from './admin-layout/admin-layout.component';
import { UserComponent } from './user/user.component';
import { BlogCreateUpdateComponent } from './blogs/blog-create-update/blog-create-update.component';
import { TaskListComponent } from './tasks/task-list/task-list.component';
import { TaskCreateUpdateComponent } from './tasks/task-create-update/task-create-update.component';
import { BlogCategoryComponent } from './blog-category/blog-category.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NzLayoutModule,
    NzMenuModule,
    NzCardModule,
    NzTableModule,
    NzCheckboxModule,
    NzButtonModule,
    NzIconModule,
    NzPopconfirmModule,
    NzModalModule,
    NzPaginationModule,
    NzFormModule,
    NzInputModule,
    NzSelectModule,
    NzNotificationModule,
    NzGridModule,
    NzPageHeaderModule,
    NzSpaceModule,
    RouterModule.forChild(AdminRoutes),
  ],
  declarations: [
    BlogListComponent,
    BlogCreateUpdateComponent,
    DashboardComponent,
    AdminLayoutComponent,
    UserComponent,
    TaskListComponent,
    TaskCreateUpdateComponent,
    BlogCategoryComponent
  ],
})
export class AdminModule {}
