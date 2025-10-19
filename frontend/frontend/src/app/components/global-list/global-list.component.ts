import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { QuizService } from 'src/app/services/quiz.service';
import { GetQuizzes } from 'src/app/models/get-quizzes.model';
import { TokenHelper } from 'src/app/helpers/token-hepler';

export interface AttemptRankingDTO {
  position: number;
  username: string;
  score: number;
  finishedAt: string;
}

@Component({
  selector: 'app-global-list',
  templateUrl: './global-list.component.html',
  styleUrls: ['./global-list.component.css']
})
export class GlobalListComponent implements OnInit {
  quizzes: GetQuizzes[] = [];
  selectedQuizId: number = 0;
  selectedPeriod: string = 'all';

  displayedColumns: string[] = ['position', 'username', 'score', 'finishedAt'];
  dataSource = new MatTableDataSource<AttemptRankingDTO>();

  totalItems = 0;
  pageSize = 5;
  pageIndex = 0;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(private quizService: QuizService) {}

  ngOnInit(): void {
    this.loadQuizzes();
    this.loadLeaderboard();
  }

  loadQuizzes() {
    this.quizService.getAll().subscribe({
      next: data => this.quizzes = data,
      error: err => console.error(err)
    });
  }

  loadLeaderboard(page: number = 1) {
  this.quizService.getLeaderboard(this.selectedQuizId, this.selectedPeriod, page, this.pageSize)
    .subscribe(res => {
      const baseUrl = 'http://localhost:5000'; 
      res.data.forEach(item => {

        if (item.picture) {
          
          if (item.picture.startsWith('/assets')) {
            item.picture = 'http://localhost:4200' + item.picture;
          }
          else 
          {
            item.picture = baseUrl + item.picture;
          }
        } else {
          
          item.picture = '/assets/images/avatar.jpg';
        }
    });

      this.dataSource.data = res.data;
      this.totalItems = res.total;
      this.pageIndex = res.page - 1;
    });
}

  onApplyFilters() {
    this.pageIndex = 0;
    this.loadLeaderboard(1);
    if (this.paginator) this.paginator.firstPage();
  }

  onPageChange(event: PageEvent) {
    this.pageSize = event.pageSize;
    this.pageIndex = event.pageIndex;
    this.loadLeaderboard(event.pageIndex + 1);
  }

  getHomeLink(): string {
  const role = TokenHelper.getRole(); 
  return role === 'Admin' ? '/admin/home' : '/user/home';
}
}


