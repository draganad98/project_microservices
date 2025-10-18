import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatTableDataSource } from '@angular/material/table';
import { QuizService } from 'src/app/services/quiz.service';
import { GetCategories } from 'src/app/models/get-categories.models';
import { Question, QuizWithQuestions } from 'src/app/models/quiz-with-questions.model';
import { UpdateQuiz } from 'src/app/models/update-quiz.model';
@Component({
  selector: 'app-update-quiz',
  templateUrl: './update-quiz.component.html',
  styleUrls: ['./update-quiz.component.css']
})
export class UpdateQuizComponent implements OnInit {
  quizForm: FormGroup;
  categories: GetCategories[] = [];
  questions: Question[] = [];
  questionsDataSource = new MatTableDataSource<Question>();
  displayedColumns: string[] = ['no', 'text', 'points', 'type', 'actions'];
  showAddQuestion = false;
  quizId!: number;

  constructor(
    private fb: FormBuilder,
    private quizService: QuizService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.quizForm = this.fb.group({
      title: ['', Validators.required],
      timeLimitSeconds: ['', [Validators.required, Validators.min(10)]],
      description: [''],
      difficultyLevel: [''],
      categoryIds: [[]]
    });
  }

  ngOnInit(): void {
    this.quizId = +this.route.snapshot.paramMap.get('id')!;
    console.log('Quiz ID to update:', this.quizId);
    this.loadCategories();
    this.loadQuiz();
  }

  loadCategories() {
    this.quizService.getCategories().subscribe({
      next: data => this.categories = data,
      error: err => console.error('Error loading categories:', err)
    });
  }

  loadQuiz() {
    this.quizService.getQuizWithQuestions(this.quizId).subscribe({
      next: (quiz: QuizWithQuestions) => {
        this.quizForm.patchValue({
          title: quiz.title,
          timeLimitSeconds: quiz.timeLimitSeconds,
          description: quiz.description,
          difficultyLevel: quiz.difficultyLevel,
          categoryIds: quiz.categoryIds
        });
        this.questions = quiz.questions;
        this.questionsDataSource.data = this.questions;
      },
      error: err => console.error('Error loading quiz:', err)
    });
  }


  openAddQuestion() {
    this.showAddQuestion = true;
  }

  closeAddQuestion() {
    this.showAddQuestion = false;
  }

  addQuestion(question: Question) {
    this.questions.push(question);
    this.questionsDataSource.data = this.questions;
    this.showAddQuestion = false;
  }

  removeQuestion(index: number) {
    const question = this.questions[index];

    if (question.id) {
      if (confirm(`Are you sure you want to delete question "${question.text}"?`)) {
        this.quizService.deleteQuestion(this.quizId, question.id).subscribe({
          next: () => {
            this.questions.splice(index, 1);
            this.questionsDataSource.data = this.questions;
          },
          error: err => alert('Failed to delete question.')
        });
      }
    } else {
      this.questions.splice(index, 1);
      this.questionsDataSource.data = this.questions;
    }
  }

  onClose() {
    this.router.navigate(['/admin/quizzes']);
  }

  
  onUpdate() {
  if (this.quizForm.invalid) {
    alert('Please fill in all required fields.');
    return;
  }

  const mappedQuestions = this.questions.map(q => ({
  ...q,
  quizId: q.quizId ?? this.quizId,
  type: q.type as 'fillBlank' | 'trueFalse' | 'multipleOne' | 'multipleMore'
}));
  const updatedQuiz: UpdateQuiz = {
    title: this.quizForm.value.title,
    description: this.quizForm.value.description,
    timeLimitSeconds: this.quizForm.value.timeLimitSeconds,
    difficultyLevel: this.quizForm.value.difficultyLevel,
    categoryIds: this.quizForm.value.categoryIds,
    questions: mappedQuestions,
    questionsNum: mappedQuestions.length
  };

  this.quizService.updateQuiz(this.quizId, updatedQuiz).subscribe({
    next: () => {
      alert('Quiz successfully updated!');
      this.router.navigate(['/admin/quizzes']);
    },
    error: (err) => {
      console.error('Error updating quiz:', err);
      alert('Failed to update quiz. Please try again.');
    }
  });
}


}

