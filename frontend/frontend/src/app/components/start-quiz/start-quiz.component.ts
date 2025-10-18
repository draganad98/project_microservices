import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { QuizService } from 'src/app/services/quiz.service';
import { QuestionFullDTO, QuizDetailsDTO } from 'src/app/models/quiz-details.model';
import { CreateUserAnswer, CreateAttempt, AttemptResponse } from 'src/app/models/attempt.model';
import { TokenHelper } from 'src/app/helpers/token-hepler';

@Component({
  selector: 'app-start-quiz',
  templateUrl: './start-quiz.component.html',
  styleUrls: ['./start-quiz.component.css']
})
export class StartQuizComponent implements OnInit, OnDestroy {
  quizId!: number;
  attemptId!: number;
  currentQuestion: QuestionFullDTO | null = null;
  currentPage = 1;
  Math = Math;
  selectedChoices: number[] = [];
  fillBlankAnswer: string = '';
  trueFalseAnswer: boolean | null = null;
  totalQuestions: number = 0;
  isLoading = true;
  isFinished = false;
  resultMessage: string = '';

  timeLimit!: number;
  timer!: number;
  interval: any;

  answers: any[] = [];
  totalAnswers = 0;
  answersPage = 1;
  answersPageSize = 5;
  constructor(private route: ActivatedRoute, private quizService: QuizService) {}

  ngOnInit(): void {
    this.quizId = Number(this.route.snapshot.paramMap.get('id'));

    this.quizService.getQuizDetails(this.quizId).subscribe((quiz: QuizDetailsDTO) => {
      this.timeLimit = quiz.timeLimitSeconds;
      this.timer = this.timeLimit;
      this.totalQuestions = quiz.questions.length;
      const now = new Date().toISOString();
      const attempt: CreateAttempt = {
        quizId: this.quizId,
        playerId: TokenHelper.getUserId(),
        startedAt: now,
        finishedAt: now
      };

      this.quizService.createAttempt(attempt).subscribe(res => {
        this.attemptId = res.id;
        this.startTimer();
        this.loadQuestion();
      });
    });
  }

  ngOnDestroy(): void {
    clearInterval(this.interval);
  }

  startTimer() {
    this.interval = setInterval(() => {
      this.timer--;

      const timerEl = document.querySelector('.timer-container');
      if (timerEl && this.timer <= 10) {
        timerEl.classList.add('warning');
      }

      if (this.timer <= 0) {
        clearInterval(this.interval);
        this.finishQuiz();
      }
    }, 1000);
  }

  loadQuestion(): void {
    this.isLoading = true;
    this.resetAnswers();

    this.quizService.getQuestionByPage(this.quizId, this.currentPage).subscribe({
      next: (data) => {
        this.currentQuestion = data;
        this.isLoading = false;
      },
      error: () => {
      
        this.currentQuestion = null;
        this.isLoading = false;
        this.finishQuiz();
      }
    });
  }

  submitAnswer(): void {
    if (!this.currentQuestion) return;

    const dto: CreateUserAnswer = {
  attemptId: this.attemptId,
  questionId: this.currentQuestion.id,
  textAnswer: this.currentQuestion.type === 'fillBlank' ? (this.fillBlankAnswer || '') : undefined,
  correct: this.currentQuestion.type === 'trueFalse' ? this.trueFalseAnswer : undefined,
  choiceIds:
    this.currentQuestion.type === 'multipleOne' || this.currentQuestion.type === 'multipleMore'
      ? [...this.selectedChoices]
      : []
};

    this.quizService.saveUserAnswer(dto).subscribe(() => this.nextQuestion());
  }

  toggleChoice(id: number): void {
    const idx = this.selectedChoices.indexOf(id);
    if (idx > -1) this.selectedChoices.splice(idx, 1);
    else this.selectedChoices.push(id);
  }

  nextQuestion(): void {
    this.currentPage++;
    this.loadQuestion();
  }

  skipQuestion(): void {
    this.nextQuestion();
  }

  finishQuiz(): void {
    if (this.isFinished) return;
    clearInterval(this.interval);

    this.quizService.finishQuiz(this.attemptId).subscribe((res: AttemptResponse) => {
    this.isFinished = true;
    this.resultMessage = `✅ Quiz completed!\n
      Number of correct answers: ${res.correctAnsNum}/${res.totalQuestions}\n
      Percentage of correct answers: ${res.correctAnsPercentage?.toFixed(2)}%\n
      Total points: ${res.score ?? 0}\n
      ⏱️ Time taken: ${res.durationSeconds} seconds`;

       this.loadAttemptAnswers();
  });
  }

  private resetAnswers(): void {
    this.selectedChoices = [];
    this.fillBlankAnswer = '';
    this.trueFalseAnswer = null;
  }

  loadAttemptAnswers(page: number = 1): void {
  this.quizService.getAttemptAnswers(this.attemptId, page, this.answersPageSize).subscribe(res => {
    this.answers = res.data;
    this.totalAnswers = res.total;
    this.answersPage = res.page;
  });
}

getCorrectAnswerText(a: any): string {
  const correctChoice = a.Choices?.find((c: any) => c.IsCorrect);
  return correctChoice ? correctChoice.Text : '';
}

get totalPages() {
  return Math.ceil(this.totalAnswers / this.answersPageSize);
}

}


