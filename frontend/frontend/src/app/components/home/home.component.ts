import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {

  isLoggedIn: boolean = false;

  constructor(private router: Router) { }

  ngDoCheck(): void {
    this.isLoggedIn = !!localStorage.getItem('jwt');
  }

  logout() {
    localStorage.removeItem('jwt');
    this.isLoggedIn = false;

    this.router.navigate(['/login']);
  }
}

