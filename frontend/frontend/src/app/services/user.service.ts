import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RegisterUser } from '../models/reqister.model';
import { LoginUser } from '../models/login.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private apiUrl = 'http://localhost:5000/users'; 

  constructor(private http: HttpClient) { }

  register(user: RegisterUser): Observable<any> {
    const formData = new FormData();
    formData.append('username', user.username);
    formData.append('email', user.email);
    formData.append('password', user.password);

    if (user.picture) {
      formData.append('picture', user.picture);
    }

    return this.http.post(`${this.apiUrl}/register`, formData);
  }

  login(user: LoginUser): Observable<string> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post(`${this.apiUrl}/authentication`, user, { headers, responseType: 'text' });
  }
}
