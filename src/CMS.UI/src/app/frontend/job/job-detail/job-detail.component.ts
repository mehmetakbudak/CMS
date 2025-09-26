import { environment } from '../../../../environments/environment';
import { Component, OnInit } from '@angular/core';
import { AppService } from '../../../services/app.service';
import { ActivatedRoute, Router } from '@angular/router';
import { SharedModule } from '../../../shared.module';
import { AuthService } from '../../../services/auth.service';
import { UserFileType } from '../../../models/enums';

@Component({
  selector: 'app-job-detail',
  imports: [SharedModule],
  templateUrl: './job-detail.component.html',
  styleUrl: './job-detail.component.css',
})
export class JobDetailComponent implements OnInit {
  id!: number;
  job: any = {};
  environment = environment;
  showApplyModal = false;
  cvList: any = [];
  selectedFileId!: number;

  constructor(
    private appService: AppService,
    private route: ActivatedRoute,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    this.route.params.subscribe((params: any) => {
      this.id = params['id'];
      this.getJobDetail();
    });
  }

  getJobDetail() {
    this.appService.get(`Job/GetDetailById/${this.id}`).subscribe((res: any) => {
      this.job = res;
    });
  }

  getUserCVList() {
    this.appService.get('UserFile/GetUserFiles').subscribe((res: any) => {
      this.cvList = res;
      this.selectedFileId = res.find((x: any) => x.isDefault)?.id;
    });
  }

  apply() {
    if (this.authService.isAuthenticated()) {
      this.showApplyModal = true;
      this.getUserCVList();
    } else {
      this.router.navigateByUrl(`/login?returnUrl=/job/${this.id}`);
    }
  }

  onSelect(e: any) {
    var uploadFile = e.files[0];
    var file = new FormData();
    file.append('file', uploadFile);
    file.append('isDefault', 'true');
    file.append('fileType', UserFileType.CV.toString());
    this.appService.post('UserFile/Create', file).subscribe((res: any) => {
      this.getUserCVList();
    });
  }

  save() {
    this.appService
      .post('UserJob/Create', {
        userFileId: this.selectedFileId,
        jobId: this.id,
      })
      .subscribe((res: any) => {
        this.showApplyModal = false;
        this.getJobDetail();
      });
  }
}
