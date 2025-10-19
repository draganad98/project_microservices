import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateQuiz } from '../models/create-quiz.model';
import { GetCategories } from '../models/get-categories.models';
import { GetQuizzes } from '../models/get-quizzes.model';
import { GetAdminQuizzes } from '../models/get-admin-quizzes';
import { QuizWithQuestions } from '../models/quiz-with-questions.model';
import { UpdateQuiz } from '../models/update-quiz.model';
import { QuizDetailsDTO, QuestionFullDTO } from '../models/quiz-details.model';
import { CreateAttempt, AttemptResponse, CreateUserAnswer } from '../models/attempt.model';
import { AttemptRankingDTO } from '../models/attempt.model';
@Injectable({
  providedIn: 'root'
})
export class QuizService {
 
  private apiUrl = 'http://localhost:5000/quiz'; 

  constructor(private http: HttpClient) {}

  createQuiz(quiz: CreateQuiz): Observable<any> {
    return this.http.post(`${this.apiUrl}/create`, quiz);
  }

  getCategories(): Observable<GetCategories[]> {
    return this.http.get<GetCategories[]>(`${this.apiUrl}/categories`);
  }

  getAllQuizzes(
    page: number = 1, 
    pageSize: number = 5, 
    selectedCategory: number[] = [], 
    selectedDifficulty: string = '', 
    keyword: string = ''
  ): Observable<{ data: GetQuizzes[], total: number, page: number, pageSize: number }> {
    let params = `?page=${page}&pageSize=${pageSize}`;
    if (selectedCategory.length > 0) {
  selectedCategory.forEach(catId => {
    params += `&categoryIds=${catId}`;
  });
}
    if (selectedDifficulty) params += `&difficulty=${selectedDifficulty}`;
    if (keyword) params += `&keyword=${keyword}`;

    return this.http.get<{ data: GetQuizzes[], total: number, page: number, pageSize: number }>(`${this.apiUrl}/quizzes${params}`);
  }

  getMyQuizzes(id: number | null, page: number = 1, pageSize: number = 5): Observable<{ data: GetAdminQuizzes[], total: number, page: number, pageSize: number }> {
    return this.http.get<{ data: GetAdminQuizzes[], total: number, page: number, pageSize: number }>(
      `${this.apiUrl}/admin/${id}?page=${page}&pageSize=${pageSize}`
    );
  }

  deleteQuiz(id: number): Observable<string> {
    return this.http.delete(`${this.apiUrl}/admin/${id}`, { responseType: 'text' });
  }

  deleteQuestion(quizId: number, questionId: number): Observable<string> {
    return this.http.delete(`${this.apiUrl}/${quizId}/questions/${questionId}`, { responseType: 'text' });
  }

  getQuizWithQuestions(id: number): Observable<QuizWithQuestions> {
    return this.http.get<QuizWithQuestions>(`${this.apiUrl}/${id}/full`);
  }

  updateQuiz(id: number, quiz: UpdateQuiz): Observable<any> {
    return this.http.put(`${this.apiUrl}/admin/${id}`, quiz);
  }

  getQuizDetails(id: number) {
    return this.http.get<QuizDetailsDTO>(`${this.apiUrl}/${id}/details`);
  }

  getQuestionByPage(quizId: number, page: number) {
    return this.http.get<QuestionFullDTO>(`${this.apiUrl}/${quizId}/question?page=${page}`);
  }
  
  createAttempt(attempt: CreateAttempt): Observable<AttemptResponse> {
    return this.http.post<AttemptResponse>(`${this.apiUrl}/attempt`, attempt);
  }

  saveUserAnswer(answer: CreateUserAnswer): Observable<any> {
    return this.http.post(`${this.apiUrl}/answer`, answer);
  }

  finishQuiz(attemptId: number) {
    return this.http.post<AttemptResponse>(`${this.apiUrl}/finish/${attemptId}`, {});
  }

  getAttemptAnswers(attemptId: number, page: number = 1, pageSize: number = 5) {
    return this.http.get<{ data: any[], total: number, page: number, pageSize: number, quizId: number }>(
      `${this.apiUrl}/attempt/${attemptId}/answers?page=${page}&pageSize=${pageSize}`
    );
  }

  getMyAttempts(userId: number | null, page: number = 1, pageSize: number = 5) {
    return this.http.get<{ data: any[], total: number, page: number, pageSize: number }>(
      `${this.apiUrl}/myattempts?userId=${userId}&page=${page}&pageSize=${pageSize}`
    );
  }

  getUserAttemptsForQuiz(userId: number | null, quizId: number) {
    return this.http.get<{ id: number, score: number, startedAt: string }[]>(
      `${this.apiUrl}/quiz/${quizId}/user-attempts/${userId}`
    );
  }

  getAll(): Observable<GetQuizzes[]> {
    return this.http.get<GetQuizzes[]>(`${this.apiUrl}/all`);
  }

  getLeaderboard(
    quizId: number = 0,
    period: string = 'all',
    page: number = 1,
    pageSize: number = 5
  ) {
    let params = `?page=${page}&pageSize=${pageSize}&timeFilter=${period}`;
    if (quizId !== 0) params += `&quizId=${quizId}`;
    return this.http.get<{ data: AttemptRankingDTO[], total: number, page: number, pageSize: number }>(
      `${this.apiUrl}/leaderboard${params}`
    );
  }
}
