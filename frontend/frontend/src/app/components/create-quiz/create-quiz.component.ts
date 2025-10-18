import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { QuizService } from 'src/app/services/quiz.service';
import { CreateQuiz } from 'src/app/models/create-quiz.model';
import { GetCategories } from 'src/app/models/get-categories.models';
import { Router } from '@angular/router';
import { Question } from 'src/app/models/question.model';
import { MatTableDataSource } from '@angular/material/table';
import { TokenHelper } from 'src/app/helpers/token-hepler';
@Component({
  selector: 'app-create-quiz',
  templateUrl: './create-quiz.component.html',
  styleUrls: ['./create-quiz.component.css']
})
export class CreateQuizComponent implements OnInit {
  quizForm: FormGroup;
  categories: GetCategories[] = [];
  questions: Question[] = [];
  showAddQuestion = false;
  displayedColumns: string[] = ['no', 'text', 'points', 'type', 'actions'];
  questionsDataSource = new MatTableDataSource<Question>();
  
  constructor(private fb: FormBuilder, private quizService: QuizService, private router: Router) {
    this.quizForm = this.fb.group({
      title: ['', Validators.required],
      timeLimitSeconds: ['', [Validators.required, Validators.min(10)]],
      description: [''],
      difficultyLevel: [''],
      categoryIds: [[]]
    });
  }

  ngOnInit(): void {
    this.quizService.getCategories().subscribe({
      next: (data) => this.categories = data,
      error: (err) => console.error('Error loading categories:', err)
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
      this.quizService.deleteQuestion(question.quizId!, question.id).subscribe({
        next: () => {
          console.log(`✅ Question ${question.id} deleted`);
          this.questions.splice(index, 1);
          this.questionsDataSource.data = this.questions;
        },
        error: (err) => {
          console.error('❌ Error deleting question:', err);
          alert('Failed to delete question.');
        }
      });
    }
  } else {
    this.questions.splice(index, 1);
    this.questionsDataSource.data = this.questions;
  }
}


  onSubmit() {
    const userId = TokenHelper.getUserId();
    if (this.quizForm.valid && this.questions.length > 0) {
      const quiz: CreateQuiz = {
        ...this.quizForm.value,
        questions: this.questions,
        creatorId: userId
      };

      this.quizService.createQuiz(quiz).subscribe({
        next: (res) => {
          console.log('✅ Quiz created:', res);
          this.router.navigate(['/admin/home']);
        },
        error: (err) => console.error('❌ Error creating quiz:', err)
      });
    } else {
      alert('Fill in the form and add at least one question!');
    }
  }

  onClose() {
    this.router.navigate(['/admin/home']);
  }
}

