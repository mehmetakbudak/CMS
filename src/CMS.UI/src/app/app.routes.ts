import { Routes } from '@angular/router';
import { FrontendLayoutComponent } from './frontend/frontend-layout/frontend-layout.component';
import { HomeComponent } from './frontend/home/home.component';
import { BlogSingleComponent } from './frontend/blog/blog-single/blog-single.component';
import { FE_BlogCategoryComponent } from './frontend/blog/blog-category/blog-category.component';
import { FE_PageComponent } from './frontend/page/page.component';
import { AdminLayoutComponent } from './admin/admin-layout/admin-layout.component';
import { DashboardComponent } from './admin/dashboard/dashboard.component';
import { BlogListComponent } from './admin/blog/blog-list/blog-list.component';
import { UserListComponent } from './admin/user/user-list/user-list.component';
import { TaskListComponent } from './admin/task/task-list/task-list.component';
import { FE_JobListComponent } from './frontend/job/job-list/job-list.component';
import { JobDetailComponent } from './frontend/job/job-detail/job-detail.component';
import { FE_ServiceListComponent } from './frontend/service/service-list/service-list.component';
import { ServiceSingleComponent } from './frontend/service/service-single/service-single.component';
import { FE_ContactComponent } from './frontend/contact/contact.component';
import { FE_TestimonialComponent } from './frontend/testimonial/testimonial.component';
import { TeamComponent } from './frontend/team/team.component';
import { RegisterComponent } from './frontend/auth/register/register.component';
import { LoginComponent } from './frontend/auth/login/login.component';
import { TaskCreateOrUpdateComponent } from './admin/task/task-create-or-update/task-create-or-update.component';
import { ProfileComponent } from './frontend/auth/profile/profile.component';
import { ChangePasswordComponent } from './frontend/auth/change-password/change-password.component';
import { BlogCreateOrUpdateComponent } from './admin/blog/blog-create-or-update/blog-create-or-update.component';
import { UserCreateOrUpdateComponent } from './admin/user/user-create-or-update/user-create-or-update.component';
import { MenuComponent } from './admin/menu/menu.component';
import { RoleComponent } from './admin/role/role.component';
import { FE_BlogListComponent } from './frontend/blog/blog-list/blog-list.component';
import { ServiceComponent } from './admin/service/service.component';
import { TestimonialComponent } from './admin/testimonial/testimonial.component';
import { CommentComponent } from './admin/comment/comment.component';
import { ContactComponent } from './admin/contact/contact.component';
import { JobListComponent } from './admin/job/job-list/job-list.component';
import { JobCreateOrUpdateComponent } from './admin/job/job-create-or-update/job-create-or-update.component';
import { TaskDefinitionComponent } from './admin/task-definition/task-definition.component';
import { AccessRightComponent } from './admin/access-right/access-right.component';
import { AuthGuard } from './helpers/auth.guard';
import { UserLayoutComponent } from './frontend/auth/user-layout/user-layout.component';
import { MyCvComponent } from './frontend/auth/my-cv/my-cv.component';
import { UserAppliedJobComponent } from './frontend/auth/user-applied-job/user-applied-job.component';
import { BlogCategoryComponent } from './admin/blog-category/blog-category.component';
import { PageComponent } from './admin/page/page.component';
import { DictionaryComponent } from './admin/dictionary/dictionary.component';
import { HomepageSliderComponent } from './admin/homepage-slider/homepage-slider.component';
import { ForgotPasswordComponent } from './frontend/auth/forgot-password/forgot-password.component';
import { SetPasswordComponent } from './frontend/auth/set-password/set-password.component';

export const routes: Routes = [
  {
    path: '',
    component: FrontendLayoutComponent,
    //canActivateChild: [()=> inject(AuthService).isAuthenticated()],
    children: [
      {
        path: '',
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
        path: 'set-password/:code',
        component: SetPasswordComponent,
      },
      {
        path: 'user',
        component: UserLayoutComponent,
        children: [
          {
            path: 'profile',
            component: ProfileComponent,
            canActivate: [AuthGuard],
          },
          {
            path: 'change-password',
            component: ChangePasswordComponent,
            canActivate: [AuthGuard],
          },
          {
            path: 'my-cv',
            component: MyCvComponent,
            canActivate: [AuthGuard],
          },
          {
            path: 'user-aplied-job',
            component: UserAppliedJobComponent,
            canActivate: [AuthGuard],
          },
        ],
      },
      {
        path: 'blog',
        component: FE_BlogListComponent,
      },
      {
        path: 'blog/:url/:id',
        component: BlogSingleComponent,
      },
      {
        path: 'blog/:url',
        component: FE_BlogCategoryComponent,
      },
      {
        path: 'page/:url',
        component: FE_PageComponent,
      },
      {
        path: 'job',
        component: FE_JobListComponent,
      },
      {
        path: 'job/:id',
        component: JobDetailComponent,
      },
      {
        path: 'service',
        component: FE_ServiceListComponent,
      },
      {
        path: 'service/:url',
        component: ServiceSingleComponent,
      },
      {
        path: 'contact',
        component: FE_ContactComponent,
      },
      {
        path: 'team',
        component: TeamComponent,
      },
      {
        path: 'testimonial',
        component: FE_TestimonialComponent,
      },
    ],
  },
  {
    path: 'admin',
    component: AdminLayoutComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: '',
        component: DashboardComponent,
      },
      {
        path: 'blog',
        children: [
          {
            path: '',
            component: BlogListComponent,
          },
          {
            path: 'create',
            component: BlogCreateOrUpdateComponent,
          },
          {
            path: 'update/:id',
            component: BlogCreateOrUpdateComponent,
          },
        ],
      },
      {
        path: 'blog-category',
        component: BlogCategoryComponent,
      },
      {
        path: 'service',
        component: ServiceComponent,
      },
      {
        path: 'user',
        children: [
          {
            path: '',
            component: UserListComponent,
          },
          {
            path: 'create',
            component: UserCreateOrUpdateComponent,
          },
          {
            path: 'update/:id',
            component: UserCreateOrUpdateComponent,
          },
        ],
      },
      {
        path: 'task',
        children: [
          {
            path: '',
            component: TaskListComponent,
          },
          {
            path: 'create',
            component: TaskCreateOrUpdateComponent,
          },
          {
            path: 'update/:id',
            component: TaskCreateOrUpdateComponent,
          },
        ],
      },
      {
        path: 'page',
        component: PageComponent,
      },
      {
        path: 'comment',
        component: CommentComponent,
      },
      {
        path: 'contact',
        component: ContactComponent,
      },
      {
        path: 'menu',
        component: MenuComponent,
      },
      {
        path: 'role',
        component: RoleComponent,
      },
      {
        path: 'testimonial',
        component: TestimonialComponent,
      },
      {
        path: 'job',
        children: [
          {
            path: '',
            component: JobListComponent,
          },
          {
            path: 'create',
            component: JobCreateOrUpdateComponent,
          },
          {
            path: 'update/:id',
            component: JobCreateOrUpdateComponent,
          },
        ],
      },
      {
        path: 'task-definition',
        component: TaskDefinitionComponent,
      },
      {
        path: 'access-right',
        component: AccessRightComponent,
      },
      {
        path: 'dictionary',
        component: DictionaryComponent,
      },
      {
        path: 'homepage-slider',
        component: HomepageSliderComponent,
      },
    ],
  },
];
