import { Routes } from '@angular/router';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { MemberLayoutComponent } from './member-layout/member-layout.component';
import { MyCommentComponent } from './my-comment/my-comment.component';
import { ProfileComponent } from './profile/profile.component';

export const MemberRoutes: Routes = [
  {
    path: '',
    component: MemberLayoutComponent,
    children: [
      {
        path: 'change-password',
        component: ChangePasswordComponent,
      },
      {
        path: 'profile',
        component: ProfileComponent,
      },
      {
        path: "my-comment",
        component: MyCommentComponent
      }
    ],
  },
];
