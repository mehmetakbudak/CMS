import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from '../../../services/app.service';
import { AlertService, alertType } from '../../../services/alert.service';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { SharedModule } from '../../../shared.module';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-blog-create-or-update',
  templateUrl: './blog-create-or-update.component.html',
  styleUrl: './blog-create-or-update.component.css',
  imports: [SharedModule],
})
export class BlogCreateOrUpdateComponent implements OnInit {
  form: FormGroup = this.formBuilder.group({
    id: [0],
    title: ['', Validators.required],
    description: [null],
    content: [null],
    blogCategoryIds: [[]],
    displayOrder: [null, [Validators.required]],
    selectedTagIds: [[]],
    published: [true, Validators.required],
    isActive: [true, Validators.required],
  });
  id: number = 0;
  blogCategories: any = [];
  tags: any = [];
  submitted = false;
  imageUrl = '';
  environment = environment;
  file: any = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private appService: AppService,
    private formBuilder: FormBuilder,
    private alertService: AlertService
  ) {}

  ngOnInit() {
    this.getBlogCategories();
    this.getTags();
    this.route.params.subscribe((params) => {
      this.id = params['id'];
      if (this.id > 0) {
        this.appService.get(`Blog/GetById/${this.id}`).subscribe((res: any) => {
          this.imageUrl = res.imageUrl;
          this.form.patchValue(res);
        });
      }
    });
  }

  getBlogCategories() {
    this.appService.get('Lookup/BlogCategories').subscribe((res: any) => {
      this.blogCategories = res;
    });
  }

  getTags() {
    this.appService.get('Tag/Get').subscribe((res: any) => {
      this.tags = res.data;
    });
  }

  get f(): { [key: string]: AbstractControl } {
    return this.form.controls;
  }

  onSelect(e: any) {
    this.file = e.files[0];
  }

  save() {
    this.submitted = true;
    if (this.form?.invalid) {
      return;
    }
    var data = this.form.value;
    var formData = new FormData();
    formData.append('id', data.id);
    formData.append('title', data.title);
    formData.append('description', data.description);
    formData.append('content', data.content);
    formData.append('displayOrder', data.displayOrder);
    formData.append('published', data.published);
    formData.append('isActive', data.isActive);
    formData.append('image', this.file);
    data.blogCategoryIds.forEach((e: any) => {
      formData.append('blogCategoryIds[]', e);
    });
    data.selectedTagIds.forEach((e: any) => {
      formData.append('selectedTagIds[]', e);
    });
    if (data.id == 0) {
      this.appService.post('Blog/Create', formData).subscribe((res: any) => {
        this.alertService.showMessage(
          'Addition was successful',
          alertType.success
        );
        setTimeout(() => {
          this.router.navigateByUrl(`/admin/blog`);
        }, 1000);
      });
    } else {
      this.appService.put('Blog/Update', formData).subscribe((res: any) => {
        this.alertService.showMessage(
          'The update was successful',
          alertType.success
        );
        setTimeout(() => {
          this.router.navigateByUrl(`/admin/blog`);
        }, 1000);
      });
    }
  }
}
