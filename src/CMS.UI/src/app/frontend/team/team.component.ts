import { Component, OnInit } from '@angular/core';
import { SharedModule } from '../../shared.module';
import { AppService } from '../../services/app.service';
import { environment } from '../../../environments/environment';

@Component({
    selector: 'app-team',
    imports: [SharedModule],
    templateUrl: './team.component.html',
    styleUrl: './team.component.css'
})
export class TeamComponent implements OnInit {
  teams: any = [];
  total: number = 0;
  skip: number = 0;
  pageSize: number = 8;
  environment = environment;

  constructor(private appService: AppService) {}

  ngOnInit() {
    this.getTeams();
  }

  getTeams() {
    this.appService
      .post('Team/GetAllActive', {
        skip: this.skip,
        pageSize: this.pageSize,
      })
      .subscribe((res: any) => {
        this.teams = res.list;
        this.total = res.total;
      });
  }

  onPageChange(event: any) {
    this.skip = event.first;
    this.pageSize = event.rows;
    this.getTeams();
  }
}
