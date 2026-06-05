import { Router } from '@angular/router';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { Auth } from '../../services/auth';

@Component({
  selector: 'app-login-page',
  imports: [FormsModule],
  templateUrl: './login-page.html',
  styleUrl: './login-page.css',
})
export class LoginPage {

  email = '';
  password = '';

  constructor(
    private authService: Auth,
    private router: Router
  ) {
  }

  login() {

    const loginData = {
      email: this.email,
      password: this.password
    };

    this.authService.login(loginData)
      .subscribe({
        next: (response: any) => {

          localStorage.setItem(
            'token',
            response.token
          );

          this.router.navigate(['/dashboard']);

        },
        error: () => {
        }
      });

  }

}
