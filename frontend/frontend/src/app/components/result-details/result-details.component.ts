import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { QuizService } from '../../services/quiz.service';
import { TokenHelper } from 'src/app/helpers/token-hepler';

@Component({
  selector: 'app-result-details',
  templateUrl: './result-details.component.html',
  styleUrls: ['./result-details.component.css']
})
export class ResultDetailsComponent implements OnInit {
  attemptId!: number;
  quizId!: number;
  answers: any[] = [];
  totalAnswers = 0;
  page = 1;
  pageSize = 5;
  Math = Math;

  userAttempts: any[] = []; 
  showGraph = false;

  constructor(
    private route: ActivatedRoute,
    private quizService: QuizService
  ) {}

  ngOnInit(): void {
    this.attemptId = Number(this.route.snapshot.paramMap.get('id'));
    this.loadAnswers();
  }

  loadAnswers(): void {
  this.quizService.getAttemptAnswers(this.attemptId, this.page, this.pageSize).subscribe(res => {
    this.answers = res.data;
    this.totalAnswers = res.total;

    
    console.log('[DEBUG] answers:', this.answers);

    this.quizId = this.answers[0]?.quizId || this.answers[0]?.question?.quizId || this.answers[0]?.attempt.quizId || res.quizId;
    console.log('[DEBUG] quizId:', this.quizId);

    if (this.quizId) {
      this.loadUserAttemptsForGraph();
    } else {
      console.warn('[DEBUG] quizId is missing, cannot load user attempts');
    }
  });
}


  changePage(newPage: number): void {
    this.page = newPage;
    this.loadAnswers();
  }

  loadUserAttemptsForGraph(): void {
    const userId = TokenHelper.getUserId();
    this.quizService.getUserAttemptsForQuiz(userId, this.quizId).subscribe((res: any[]) => {
  console.log('[DEBUG] API response:', res);
  this.userAttempts = res;
  console.log('[DEBUG] userAttempts set:', this.userAttempts);
});
  }

  getSelectedChoices(answer: any): string {
    return answer.choices
      .filter((c: any) => answer.userChoiceIds.includes(c.id))
      .map((c: any) => c.text)
      .join(', ');
  }

  getCorrectChoices(answer: any): string {
    return answer.choices
      .filter((c: any) => c.isCorrect)
      .map((c: any) => c.text)
      .join(', ');
  }
}

