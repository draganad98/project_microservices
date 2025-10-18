import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from 'src/app/services/user.service';
import { RegisterUser } from 'src/app/models/reqister.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  registerForm: FormGroup;
  selectedFile: File | null = null;
  previewUrl: string = 'assets/images/avatar.jpg'; 
  hide: boolean = true;

  constructor(private fb: FormBuilder, private userService: UserService, private router: Router) {
    this.registerForm = this.fb.group({
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  onFileSelected(event: any) {
  if (event.target.files && event.target.files[0]) {
    this.selectedFile = event.target.files[0];

    if (this.selectedFile) {
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.previewUrl = e.target.result;
      };
      reader.readAsDataURL(this.selectedFile);
    }
  }
}


  onSubmit() {
  if (this.registerForm.valid) {
    const user: RegisterUser = {
      ...this.registerForm.value,
      picture: this.selectedFile || undefined
    };

    this.userService.register(user).subscribe({
      next: (res) => {
        alert('Registration successful! You can now log in.');
        this.router.navigate(['/login']);
      },
      error: (err) => {
        console.error('Registration failed', err);

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

togglePasswordVisibility() {
    this.hide = !this.hide;
  }

}

