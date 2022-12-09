import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { SourceType } from '../storage/enums/source-type';
import { SourceCommentModel } from '../storage/models/comment.model';

@Injectable()
export class CommentService {
  constructor(private http: HttpClient) {}

  getMemberComments(pageNumber?: number, pageSize?: number, type?: number) {
    var url = `${environment.ApiUrl}Comment/GetUserComments`;
    if (type) {
      url = `${url}/${type}`;
    }
    url = `${url}?pageNumber=${pageNumber}&pageSize=${pageSize}`;
    return this.http.get(url);
  }
}
