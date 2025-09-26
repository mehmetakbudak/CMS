import { AppService } from './../../services/app.service';
import { Component, OnInit } from '@angular/core';
import { SharedModule } from '../../shared.module';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';

@Component({
  selector: 'app-contact',
  imports: [SharedModule],
  templateUrl: './contact.component.html',
  styleUrl: './contact.component.css',
})
export class FE_ContactComponent implements OnInit {
  contactCategories: any = [];
  submitted = false;
  form: FormGroup = this.formBuilder.group({
    name: [null, Validators.required],
    surname: [null, Validators.required],
    emailAddress: [null, [Validators.required, Validators.email]],
    contactCategoryId: [null, Validators.required],
    message: [null, Validators.required],
  });

  constructor(
    private appService: AppService,
    private formBuilder: FormBuilder
  ) {}

  ngOnInit(): void {
    this.getContactCategories();
  }

  get f(): { [key: string]: AbstractControl } {
    return this.form.controls;
  }

  getContactCategories() {
    this.appService.get('Lookup/ContactCategories').subscribe((res: any) => {
      this.contactCategories = res;
    });
  }

  save() {
    this.submitted = true;
    if (this.form?.invalid) {
      return;
    }
    this.appService
      .post(`Contact/Create`, this.form.value)
      .subscribe((res: any) => {
        this.form.reset();
        this.submitted = false;
        this.form.clearAsyncValidators();
      });
  }
}
