import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { AuthInterceptor } from './helpers/auth.interceptor';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { AddQuestionComponent } from './components/add-question/add-question.component';
import { AdminHomeComponent } from './components/admin-home/admin-home.component';
import { CreateQuizComponent } from './components/create-quiz/create-quiz.component';
import { GlobalListComponent } from './components/global-list/global-list.component';
import { LoginComponent } from './components/login/login.component';
import { MyResultsComponent } from './components/my-results/my-results.component';
import { QuizzesListComponent } from './components/quizzes-list/quizzes-list.component';
import { RegisterComponent } from './components/register/register.component';
import { UpdateQuizComponent } from './components/update-quiz/update-quiz.component';
import { UserHomeComponent } from './components/user-home/user-home.component';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatTabsModule } from '@angular/material/tabs';
import { MatCardModule } from '@angular/material/card';
import { MatSelectModule } from '@angular/material/select'; 
import { MatTableModule } from '@angular/material/table';
import { HttpClientModule } from '@angular/common/http';
import { MyQuizzesComponent } from './components/my-quizzes/my-quizzes.component';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatIconModule } from '@angular/material/icon';
import { MatRadioModule } from '@angular/material/radio';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { StartQuizComponent } from './components/start-quiz/start-quiz.component';
import { ResultDetailsComponent } from './components/result-details/result-details.component';
import { ProgressGraphComponent } from './components/progress-graph/progress-graph.component';



@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    AddQuestionComponent,
    AdminHomeComponent,
    CreateQuizComponent,
    GlobalListComponent,
    LoginComponent,
    MyResultsComponent,
    QuizzesListComponent,
    RegisterComponent,
    UpdateQuizComponent,
    UserHomeComponent,
    MyQuizzesComponent,
    StartQuizComponent,
    ResultDetailsComponent,
    ProgressGraphComponent
  ],
  imports: [
    HttpClientModule,
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    MatTabsModule,
    MatCardModule,
    MatSelectModule,
    FormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatIconModule,
    MatRadioModule,
    MatCheckboxModule
   
   
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
