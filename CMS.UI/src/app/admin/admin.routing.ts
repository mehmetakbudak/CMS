import { Routes } from '@angular/router';
import { AdminLayoutComponent } from './admin-layout/admin-layout.component';
import { BlogCreateUpdateComponent } from './blogs/blog-create-update/blog-create-update.component';
import { BlogListComponent } from './blogs/blog-list/blog-list.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { UserComponent } from './user/user.component';

export const AdminRoutes: Routes = [
  {
    path: '',
    component: AdminLayoutComponent,
    children: [
      {
        path: '',
        pathMatch: 'full',
        component: DashboardComponent,
      },
      {
        path: 'blog',
        component: BlogListComponent,
      },
      {
        path: 'blog/add',
        component: BlogCreateUpdateComponent,
      },
      {
        path: 'blog/edit/:id',
        component: BlogCreateUpdateComponent,
      },
      {
        path: 'user',
        component: UserComponent,
      },
    ],
  },
];
