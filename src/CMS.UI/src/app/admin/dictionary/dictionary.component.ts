import { Component, OnInit } from '@angular/core';
import { SharedModule } from '../../shared.module';
import { AlertService, alertType } from '../../services/alert.service';
import {
  AbstractControl,
  FormArray,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { AppService } from '../../services/app.service';
import { ConfirmationService } from 'primeng/api';

class LanguageValueFilterDto {
  code!: string;
  languageValues: LanguageValueDto[] = [];
}
class LanguageValueDto {
  languageName!: string;
  languageId!: number;
  value!: string;
}

@Component({
  selector: 'app-dictionary',
  templateUrl: './dictionary.component.html',
  styleUrl: './dictionary.component.css',
  imports: [SharedModule],
})
export class DictionaryComponent implements OnInit {
  form: FormGroup = this.formBuilder.group({
    id: [0],
    code: ['', Validators.required],
    values: this.formBuilder.array([]),
  });

  totalRecords!: number;
  rows = 5;
  first = 0;
  menuItems: any = [];
  gridMenuItems: any = [];
  languages: any = [];
  list: any = [];
  loading = false;
  selectedData!: any;
  filterForm!: LanguageValueFilterDto;
  submitted = false;
  isVisible = false;
  title = '';
  screenWidth: number = window.innerWidth;

  constructor(
    private alertService: AlertService,
    private confirmationService: ConfirmationService,
    private appService: AppService,
    private formBuilder: FormBuilder
  ) {}

  ngOnInit() {
    this.filterForm = new LanguageValueFilterDto();
    this.getList();
    this.getLanguages();
    this.getMenuItems();
    this.getGridMenuItems();
    console.log(this.screenWidth);
    this.appService.onLangChange().subscribe(() => {
      this.getMenuItems();
      this.getGridMenuItems();
    });
  }

  getMenuItems() {
    this.menuItems = [
      {
        label: this.appService.getTranslate('Add'),
        command: () => {
          this.form.reset();
          this.isVisible = true;
          this.getLanguages();
          this.form.patchValue({ id: 0 });
          this.title = this.appService.getTranslate('Add');
        },
      },
    ];
  }

  getGridMenuItems() {
    this.gridMenuItems = [
      {
        label: this.appService.getTranslate('Edit'),
        command: () => {
          this.edit(this.selectedData.id);
        },
      },
      {
        label: this.appService.getTranslate('Delete'),
        command: (e: any) => {
          this.delete(e);
        },
      },
    ];
  }

  getList() {
    var data = {
      skip: this.first,
      take: this.rows,
      code: this.filterForm.code,
      languageValues: this.filterForm.languageValues.map((x: any) => {
        return {
          languageId: x.languageId,
          value: x.value,
        };
      }),
    };
    this.appService.post(`LanguageValue/Get`, data).subscribe((res: any) => {
      this.list = res;
      this.totalRecords = res.totalCount;
    });
  }

  getLanguages() {
    this.appService.get('Language/Get').subscribe((res: any) => {
      this.languages = res.data;
      this.filterForm.languageValues = res.data.map((x: any) => {
        return {
          languageId: x.id,
          languageName: x.name,
        };
      });
      this.values.clear();
      this.languages.forEach((lang: any) => {
        this.values.push(this.createTranslation(lang));
      });
    });
  }

  getLanguageName(e: any) {
    const lang = this.languages.find(
      (x: any) => x.id == e.controls.languageId.value
    );
    return lang ? lang.name : '';
  }

  createTranslation(lang: any): FormGroup {
    return this.formBuilder.group({
      languageId: [lang.id],
      value: [null, Validators.required],
    });
  }

  get values(): FormArray {
    return this.form?.get('values') as FormArray;
  }

  get f(): { [key: string]: AbstractControl } {
    return this.form.controls;
  }

  pageChange(e: any) {
    this.first = e.first;
    this.rows = e.rows;
    this.getList();
  }

  menuToggle(menu: any, e: any, data: any) {
    this.gridMenuItems.forEach((menuItem: any) => {
      menuItem.data = data;
    });
    this.selectedData = data;
    menu.toggle(e);
  }

  reset() {
    this.filterForm = new LanguageValueFilterDto();
    this.getLanguages();
    this.getList();
  }

  search() {
    this.getList();
  }

  edit(id: any) {
    this.appService.get(`LanguageValue/GetById/${id}`).subscribe((res: any) => {
      this.isVisible = true;
      this.form.patchValue(res);
      this.title = this.appService.getTranslate('Edit');
    });
  }

  delete(event: any) {
    this.confirmationService.confirm({
      target: event.target as EventTarget,
      message: this.appService.getTranslate('Alert.Delete'),
      header: this.appService.getTranslate('Delete'),
      icon: 'pi pi-exclamation-triangle',
      acceptLabel: this.appService.getTranslate('Yes'),
      rejectLabel: this.appService.getTranslate('No'),
      acceptIcon: 'none',
      rejectIcon: 'none',
      rejectButtonStyleClass: 'p-button-text',
      accept: () => {
        this.appService
          .delete(`LanguageValue/Delete`, this.selectedData.id)
          .subscribe((res: any) => {
            this.getList();
            this.alertService.showMessage(
              'Deletion was successful',
              alertType.success
            );
          });
      },
    });
  }

  save() {
    this.submitted = true;
    if (this.form?.invalid) {
      return;
    }
    if (this.form.value.id == 0) {
      this.appService
        .post('LanguageValue/Create', this.form.value)
        .subscribe((res: any) => {
          this.getList();
          this.alertService.showMessage(
            this.appService.getTranslate('Alert.Create'),
            alertType.success
          );
          this.isVisible = false;
          localStorage.removeItem('translations');
        });
    } else {
      this.appService
        .put('LanguageValue/Update', this.form.value)
        .subscribe((res: any) => {
          this.getList();
          this.alertService.showMessage(
            this.appService.getTranslate('Alert.Update'),
            alertType.success
          );
          this.isVisible = false;
          localStorage.removeItem('translations');
        });
    }
  }
}
