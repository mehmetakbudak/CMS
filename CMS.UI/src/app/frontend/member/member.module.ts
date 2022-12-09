import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzListModule } from 'ng-zorro-antd/list';
import { NzTableModule } from 'ng-zorro-antd/table';
import { CommentService } from 'src/app/services/comment.service';
import { ChangePasswordComponent } from './change-password/change-password.component';
import { MemberLayoutComponent } from './member-layout/member-layout.component';
import { MemberRoutes } from './member.routing';
import { MyCommentComponent } from './my-comment/my-comment.component';
import { ProfileComponent } from './profile/profile.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,    
    NzGridModule,
    NzCardModule,
    NzListModule,
    NzIconModule,
    NzTableModule,
    NzButtonModule,
    RouterModule.forChild(MemberRoutes)],
  declarations: [
    MemberLayoutComponent,
    ChangePasswordComponent,
    ProfileComponent,
    MyCommentComponent
  ],
  providers: [
    CommentService
  ],
})
export class MemberModule {}
