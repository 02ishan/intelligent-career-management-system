import { Routes } from '@angular/router';
import { authGuard } from './guards/auth-guard';
import { LoginPage } from './pages/login-page/login-page';
import { RegisterPage } from './pages/register-page/register-page';
import { Dashboard } from './pages/dashboard/dashboard';
import { JobsPage } from './pages/jobs-page/jobs-page';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: 'login',
    component: LoginPage
  },
  {
    path: 'register',
    component: RegisterPage
  },
  {
    path: 'dashboard',
    component: Dashboard,
    canActivate: [authGuard]
  },
  {
    path: 'jobs',
    component: JobsPage,
    canActivate: [authGuard]
  }
];
