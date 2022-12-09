import { Component, OnInit } from '@angular/core';
import { CommentService } from 'src/app/services/comment.service';

@Component({
  selector: 'app-my-comment',
  templateUrl: './my-comment.component.html',
  styleUrls: ['./my-comment.component.css'],
})
export class MyCommentComponent implements OnInit {
  expandSet = new Set<number>();
  list: any = [];
  loading: boolean = false;
  total = 0;
  pageNumber = 1;
  pageSize = 5;

  constructor(private commentService: CommentService) {}

  ngOnInit() {
    this.getAll();
  }

  getAll() {
    this.commentService
      .getMemberComments(this.pageNumber, this.pageSize)
      .subscribe((res: any) => {
        this.list = res.data;
        this.total = res.total;
        this.pageNumber = res.pageNumber;
        this.pageSize = res.pageSize;
      });
  }

  onQueryParamsChange(e: any) {
    this.pageNumber = e.pageIndex;
    this.pageSize = e.pageSize;
    this.getAll();
  }

  onExpandChange(id: number, checked: boolean): void {
    if (checked) {
      this.expandSet.add(id);
    } else {
      this.expandSet.delete(id);
    }
  }

  deleteComment(id: number) {}
}
