import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-navbar',
  imports: [
    CommonModule,
    RouterLink
  ],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class Navbar implements OnInit {

  role = '';

  showNavbar = true;

  constructor(
    private router: Router
  ) {
  }

  ngOnInit(): void {

    this.checkNavbarVisibility();

    this.router.events
      .pipe(
        filter(event => event instanceof NavigationEnd)
      )
      .subscribe(() => {

        this.checkNavbarVisibility();

      });

    const token = localStorage.getItem('token');

    if (token) {

      const payload = JSON.parse(
        atob(token.split('.')[1])
      );

      this.role =
        payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

    }

  }

  checkNavbarVisibility() {

    const currentUrl = this.router.url;

    this.showNavbar =
      currentUrl !== '/login' &&
      currentUrl !== '/register';

  }

  logout() {

    localStorage.removeItem('token');

    this.router.navigate(['/login']);

  }

}
