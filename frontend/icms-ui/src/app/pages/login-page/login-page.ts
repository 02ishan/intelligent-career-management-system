import { Router } from '@angular/router';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { Auth } from '../../services/auth';
import { Navbar } from '../../components/navbar/navbar';

@Component({
  selector: 'app-login-page',
  imports: [FormsModule, Navbar],
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

          const payload = JSON.parse(
            atob(response.token.split('.')[1])
          );

          const role =
            payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

          if (role === 'Student') {

            this.router.navigate(['/jobs']);

          }
          else {

            this.router.navigate(['/dashboard']);

          }

        },
        error: () => {
        }
      });

  }

}
