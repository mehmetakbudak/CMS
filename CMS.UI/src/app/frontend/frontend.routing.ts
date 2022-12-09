import { Routes } from '@angular/router';
import { BlogCategoryComponent } from './blogs/blog-category/blog-category.component';
import { BlogDetailComponent } from './blogs/blog-detail/blog-detail.component';
import { BlogListComponent } from './blogs/blog-list/blog-list.component';
import { HomeComponent } from './home/home.component';
import { LayoutComponent } from './layout/layout.component';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { ForgotPasswordComponent } from './auth/forgot-password/forgot-password.component';

export const FrontendRoutes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      {
        path: '',
        pathMatch: 'full',
        component: HomeComponent,
      },
      {
        path: 'login',
        component: LoginComponent,
      },
      {
        path: 'register',
        component: RegisterComponent,
      },
      {
        path: 'forgot-password',
        component: ForgotPasswordComponent,
      },
      {
        path: 'blog',
        component: BlogListComponent,
      },
      {
        path: 'blog/:categoryName',
        component: BlogCategoryComponent,
      },
      {
        path: 'blog/:url/:id',
        component: BlogDetailComponent,
      },
    ],
  },
];
