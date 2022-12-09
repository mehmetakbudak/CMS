import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import {
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { SourceType } from '../../../storage/enums/source-type';

@Component({
  selector: 'app-blog-detail',
  templateUrl: './blog-detail.component.html',
  styleUrls: ['./blog-detail.component.css'],
})
export class BlogDetailComponent implements OnInit {
  replyCommentForm!: UntypedFormGroup;
  blog: any;
  urlParams: any;
  comments: any = [];
  mostReadList: any = [];
  visibleReplyModal = false;
  replyComment: any;

  constructor(
    private http: HttpClient,
    private route: ActivatedRoute,
    private router: Router,
    private formBuilder: UntypedFormBuilder
  ) {
    this.replyCommentForm = this.formBuilder.group({
      comment: [null, [Validators.required, Validators.maxLength(500)]],
    });
    this.urlParams = this.route.params;
  }

  ngOnInit() {
    this.getById();
    this.getComments();
    this.getMostRead();
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  getById() {
    this.http
      .get(`${environment.ApiUrl}blog/${this.urlParams.value.id}`)
      .subscribe((res: any) => {
        this.blog = res;
      });
  }

  getMostRead() {
    this.http
      .get(`${environment.ApiUrl}blog/mostread`)
      .subscribe((res: any) => {
        this.mostReadList = res;
      });
  }

  getComments() {
    var data = {
      sourceType: SourceType.Blog,
      sourceId: this.urlParams.value.id,
    };
    this.http
      .post(`${environment.ApiUrl}comment/getsourcecomments`, data)
      .subscribe((res: any) => {
        this.comments = res;
      });
  }

  reply(comment: any) {
    this.visibleReplyModal = true;
    this.replyComment = comment;
  }

  saveReply() {
    if (this.replyCommentForm.valid) {
    } else {
      Object.values(this.replyCommentForm.controls).forEach((control) => {
        if (control.invalid) {
          control.markAsDirty();
          control.updateValueAndValidity({ onlySelf: true });
        }
      });
    }
  }
}
