import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent} from './components/register/register.component'
import { AdminHomeComponent } from './components/admin-home/admin-home.component';
import { QuizzesListComponent } from './components/quizzes-list/quizzes-list.component';
import { CreateQuizComponent } from './components/create-quiz/create-quiz.component';
import { GlobalListComponent } from './components/global-list/global-list.component';
import { UserHomeComponent } from './components/user-home/user-home.component';
import { MyResultsComponent } from './components/my-results/my-results.component';
import { UpdateQuizComponent } from './components/update-quiz/update-quiz.component';
import { MyQuizzesComponent } from './components/my-quizzes/my-quizzes.component';
import { StartQuizComponent } from './components/start-quiz/start-quiz.component';
import { ResultDetailsComponent } from './components/result-details/result-details.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    children: [
      { path: '', redirectTo: 'login', pathMatch: 'full' },
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent},
      { path: 'admin/home', component: AdminHomeComponent},
      { path: 'admin/quizzes', component: MyQuizzesComponent},
      { path: 'admin/create', component: CreateQuizComponent},
      { path: 'admin/list', component: GlobalListComponent},
      { path: 'user/home', component: UserHomeComponent},
      { path: 'list', component: GlobalListComponent},
      { path: 'user/quizzes', component: QuizzesListComponent},
      { path: 'user/results', component: MyResultsComponent},
      { path: 'admin/update/:id', component: UpdateQuizComponent },
      { path: 'quiz/:id/start', component: StartQuizComponent },
      {path: 'result-details/:id', component: ResultDetailsComponent },
      { path: 'user/list', component: GlobalListComponent},
      

    ]
  },   
  { path: '**', redirectTo: '' } 
];


@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
