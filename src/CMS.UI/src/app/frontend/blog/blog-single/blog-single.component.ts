import { AuthService } from './../../../services/auth.service';
import { environment } from './../../../../environments/environment';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppService } from '../../../services/app.service';
import { SourceType } from '../../../models/enums';
import { TreeNode } from 'primeng/api';
import { AlertService, alertType } from '../../../services/alert.service';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { SharedModule } from '../../../shared.module';

@Component({
  selector: 'app-blog-single',
  templateUrl: './blog-single.component.html',
  styleUrls: ['./blog-single.component.scss'],
  imports: [SharedModule],
})
export class BlogSingleComponent implements OnInit {
  form: FormGroup = this.formBuilder.group({
    comment: ['', [Validators.required]],
  });
  id!: number;
  blog: any = {};
  environment = environment;
  mostReads: any = [];
  comments: any = [];
  showCommentModal = false;
  headerCommentModal = '';
  selectedComment: any = null;
  comment = '';
  submitted = false;
  isAuthenticated = false;

  constructor(
    private route: ActivatedRoute,
    private appService: AppService,
    private authService: AuthService,
    private alertService: AlertService,
    private formBuilder: FormBuilder
  ) {}

  ngOnInit() {
    this.isAuthenticated = this.authService.isAuthenticated();
    this.route.params.subscribe((params: any) => {
      this.id = params['id'];
      this.getMostReads();
      this.getDetails();
      this.getComments();
      this.seen();
    });
  }

  getDetails() {
    this.appService.get(`Blog/GetDetailById/${this.id}`).subscribe((res: any) => {
      this.blog = res;
    });
  }

  getMostReads() {
    this.appService.get('Blog/MostRead').subscribe((res: any) => {
      this.mostReads = res;
    });
  }

  getComments() {
    this.appService
      .post('Comment/GetSourceComments', {
        sourceType: SourceType.Blog,
        sourceId: this.id,
      })
      .subscribe((res: any) => {
        this.comments = res;
        this.expandAll();
      });
  }

  get f(): { [key: string]: AbstractControl } {
    return this.form.controls;
  }

  expandAll() {
    this.comments.forEach((node: any) => {
      this.expandRecursive(node, true);
    });
  }

  private expandRecursive(node: TreeNode, isExpand: boolean) {
    node.expanded = isExpand;
    if (node.children) {
      node.children.forEach((childNode) => {
        this.expandRecursive(childNode, isExpand);
      });
    }
  }

  add() {
    if (this.authService.isAuthenticated()) {
      this.form.reset();
      this.form.clearAsyncValidators();
      this.showCommentModal = true;
      this.headerCommentModal = 'New Comment';
      this.selectedComment = null;
    } else {
      this.alertService.showMessage('Please sign in!', alertType.info);
    }
  }

  reply(data: any) {
    if (this.authService.isAuthenticated()) {
      this.form.reset();
      this.form.clearAsyncValidators();
      this.showCommentModal = true;
      this.headerCommentModal = 'Reply Comment';
      this.selectedComment = data;
    } else {
      this.alertService.showMessage('Please sign in!', alertType.info);
    }
  }

  seen() {
    this.appService.put('Blog/Seen', {
      id: this.id,
    });
  }

  save() {
    this.submitted = true;
    if (this.form?.invalid) {
      return;
    }
    this.appService
      .post('Comment/Create', {
        sourceId: this.id,
        sourceType: SourceType.Blog,
        description: this.form.value.comment,
        parentId: this.selectedComment ? this.selectedComment.id : null,
      })
      .subscribe((res: any) => {
        this.showCommentModal = false;
        this.alertService.showMessage(res.message, alertType.success);
      });
  }
}
