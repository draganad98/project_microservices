import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from 'src/app/services/user.service';
import { Router } from '@angular/router';
import { TokenHelper } from 'src/app/helpers/token-hepler';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginForm: FormGroup;
  hide: boolean = true; 

  constructor(private fb: FormBuilder, private userService: UserService, private router: Router) {
    this.loginForm = this.fb.group({
      emailOrUsername: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  togglePasswordVisibility() {
    this.hide = !this.hide;
  }

  onSubmit() {
    if (this.loginForm.valid) {
      this.userService.login(this.loginForm.value).subscribe({
        next: (token: string) => {
          localStorage.setItem('jwt', token);
          console.log('Login success, token saved:', token);

          const role = TokenHelper.getRole();
          console.log('Detected role:', role);

          if (role === 'Admin') {
            this.router.navigate(['/admin/home']);
          } else if (role === 'User') {
            this.router.navigate(['/user/home']);
          } else {
            this.router.navigate(['/']);
          }
        },
        error: (err) => {
          console.error('Login failed', err);

          if (err.status === 400 && err.error) {
          alert(err.error); 
        } else {
          console.log(err.error)
          alert('Something went wrong!');
        }
        }
      });
    }
  }
}

