import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { QuizService } from 'src/app/services/quiz.service';
import { GetCategories } from 'src/app/models/get-categories.models';
import { GetQuizzes } from 'src/app/models/get-quizzes.model';
import { Router } from '@angular/router';
@Component({
  selector: 'app-quizzes-list',
  templateUrl: './quizzes-list.component.html',
  styleUrls: ['./quizzes-list.component.css']
})
export class QuizzesListComponent implements OnInit {
  dataSource = new MatTableDataSource<GetQuizzes>();
  categories: GetCategories[] = [];
  selectedCategory: number[] = [];
  selectedDifficulty: string = '';
  keyword: string = '';

  selectedQuiz: GetQuizzes | null = null;

  totalQuizzes = 0;
  pageSize = 5;
  pageIndex = 0;

  displayedColumns: string[] = [
    'title', 'categories', 'description', 'timeLimitSeconds', 'difficulty', 'questionsNum', 'start'
  ];

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(private quizService: QuizService, private router: Router) {}

  ngOnInit(): void {
    this.loadCategories();
    this.loadQuizzes();
  }

  

  loadCategories() {
    this.quizService.getCategories().subscribe({
      next: data => this.categories = data,
      error: err => console.error('Error loading categories:', err)
    });
  }

  loadQuizzes(page: number = 1) {
  this.quizService.getAllQuizzes(
    page,
    this.pageSize,
    this.selectedCategory,
    this.selectedDifficulty,
    this.keyword
  ).subscribe(res => {
    this.dataSource.data = res.data;   
    this.totalQuizzes = res.total;     
    this.pageIndex = res.page - 1;     
  });
}


  applyFilters() {
    this.pageIndex = 0;              
    this.loadQuizzes(1);             
    if (this.paginator) this.paginator.firstPage();
  }

  onPageChange(event: PageEvent) {
    this.pageSize = event.pageSize;
    this.pageIndex = event.pageIndex;
    this.loadQuizzes(event.pageIndex + 1); 
  }

  getCategoryNames(quiz: GetQuizzes): string {
    return quiz.categoryIds
      .map(id => this.categories.find(c => c.id === id)?.name)
      .filter(name => !!name)
      .join(', ');
  }

  
  
  onStart(quiz: GetQuizzes) {
    this.router.navigate([`/quiz/${quiz.id}/start`]);
  }
}
