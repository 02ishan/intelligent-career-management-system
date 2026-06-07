import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Auth } from '../../services/auth';
import { Navbar } from '../../components/navbar/navbar';

@Component({
  selector: 'app-register-page',
  imports: [FormsModule, Navbar],
  templateUrl: './register-page.html',
  styleUrl: './register-page.css',
})
export class RegisterPage {

  register() {

  const registerData = {
    fullName: this.fullName,
    email: this.email,
    password: this.password,
    role: this.role
  };

  this.authService.register(registerData)
    .subscribe({
      next: (response) => {
        console.log(response);
      },
      error: (error) => {
        console.log(error);
      }
    });

}

  fullName = '';
  email = '';
  password = '';
  role = 'Student';

  constructor(
    private authService: Auth,
    private router: Router
  ) {
  }

}
