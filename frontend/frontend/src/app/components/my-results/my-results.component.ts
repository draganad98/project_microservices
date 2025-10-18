import { Component, OnInit } from '@angular/core';
import { TokenHelper } from 'src/app/helpers/token-hepler';
import { QuizService } from 'src/app/services/quiz.service';

export interface MyAttempt {
  id: number;
  title: string;
  startedAt: Date;
  score: number;
  percentage: number;
}

@Component({
  selector: 'app-my-results',
  templateUrl: './my-results.component.html',
  styleUrls: ['./my-results.component.css']
})
export class MyResultsComponent implements OnInit {
  displayedColumns: string[] = ['title', 'datetime', 'score', 'duration', 'percentage', 'details'];
  results: MyAttempt[] = [];

  pageIndex = 0;
  pageSize = 5;
  totalResults = 0;

  constructor(private quizService: QuizService) {}

  ngOnInit() {
    this.loadAttempts(1);
  }

  loadAttempts(page: number) {
    const userId = TokenHelper.getUserId();
    this.quizService.getMyAttempts(userId, page, this.pageSize).subscribe(res => {
      this.results = res.data;
      this.totalResults = res.total;
      this.pageIndex = res.page - 1;
    });
  }

  onPageChange(event: any) {
    this.pageSize = event.pageSize;
    this.pageIndex = event.pageIndex;
    this.loadAttempts(event.pageIndex + 1);
  }

  onDetails(attempt: MyAttempt) {
    console.log('Details for:', attempt);
  }
}

