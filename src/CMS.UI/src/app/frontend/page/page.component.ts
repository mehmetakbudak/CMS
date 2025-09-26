import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from '../../services/app.service';
import { SharedModule } from '../../shared.module';

@Component({
  selector: 'app-page',
  templateUrl: './page.component.html',
  styleUrls: ['./page.component.scss'],
  imports: [SharedModule],
})
export class FE_PageComponent implements OnInit {
  items: any;
  page: any;
  menuName: any;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private appService: AppService
  ) {}

  ngOnInit() {
    this.route.params.subscribe((params: any) => {
      this.appService.get(`Page/GetByUrl?url=/page/${params['url']}`).subscribe({
        next: (res: any) => {
          this.page = res;
        },
        error: (err: any) => {
          this.router.navigateByUrl('/');
        },
      });
    });
  }
}
