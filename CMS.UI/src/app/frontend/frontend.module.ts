import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzListModule } from 'ng-zorro-antd/list';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzCommentModule } from 'ng-zorro-antd/comment';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzNotificationModule } from 'ng-zorro-antd/notification';

import { FrontendRoutes } from './frontend.routing';
import { BlogListComponent } from './blogs/blog-list/blog-list.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './auth/login/login.component';
import { LayoutComponent } from './layout/layout.component';
import { BlogCategoryComponent } from './blogs/blog-category/blog-category.component';
import { BlogDetailComponent } from './blogs/blog-detail/blog-detail.component';
import { RegisterComponent } from './auth/register/register.component';
import { ForgotPasswordComponent } from './auth/forgot-password/forgot-password.component';

@NgModule({
  imports: [
    CommonModule,
    NzFormModule,
    NzInputModule,
    FormsModule,
    ReactiveFormsModule,
    NzButtonModule,
    NzCardModule,
    NzGridModule,
    NzLayoutModule,
    NzListModule,
    NzDividerModule,
    NzIconModule,
    NzBreadCrumbModule,
    NzCommentModule,
    NzAvatarModule,
    NzModalModule,
    NzNotificationModule,
    RouterModule.forChild(FrontendRoutes),
  ],
  declarations: [
    BlogListComponent,
    BlogCategoryComponent,
    BlogDetailComponent,
    HomeComponent,
    LayoutComponent,
    LoginComponent,
    RegisterComponent,
    ForgotPasswordComponent
  ],
})
export class FrontendModule { }
