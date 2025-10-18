import { Component, OnInit, ViewChild } from '@angular/core';
import { GetAdminQuizzes } from 'src/app/models/get-admin-quizzes';
import { QuizService } from 'src/app/services/quiz.service';
import { TokenHelper } from 'src/app/helpers/token-hepler';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Router } from '@angular/router';

@Component({
  selector: 'app-my-quizzes',
  templateUrl: './my-quizzes.component.html',
  styleUrls: ['./my-quizzes.component.css']
})
export class MyQuizzesComponent implements OnInit {
  quizzes: GetAdminQuizzes[] = [];
  selectedQuiz: GetAdminQuizzes | null = null;

  totalQuizzes = 0;
  pageSize = 5;
  currentPage = 1;

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(private quizService: QuizService, private router: Router) { }

  ngOnInit(): void {
    this.loadQuizzes();
  }

  loadQuizzes(page: number = 1) {
    const userId = TokenHelper.getUserId();
    this.quizService.getMyQuizzes(userId, page, this.pageSize).subscribe({
      next: (res) => {
        this.quizzes = res.data;
        this.totalQuizzes = res.total;
        this.currentPage = res.page;
      },
      error: (err) => console.error('Error loading quizzes:', err)
    });
  }

  onPageChange(event: PageEvent) {
    this.pageSize = event.pageSize;
    this.loadQuizzes(event.pageIndex + 1);
  }

  onDelete(quizId: number) {
    if (!confirm('Are you sure you want to delete this quiz?')) return;

    this.quizService.deleteQuiz(quizId).subscribe({
      next: () => {
        this.loadQuizzes(this.currentPage); 
      },
      error: (err) => {
        console.error('Error deleting quiz:', err);
        alert('Failed to delete quiz.');
      }
    });
  }

  
  onUpdate(quiz: GetAdminQuizzes) {
  this.router.navigate(['/admin/update', quiz.id]);
}
  onStart(quiz: GetAdminQuizzes) { console.log('Start quiz', quiz); }

  onClose() {
    this.router.navigate(['/admin/home']);
  }
}
