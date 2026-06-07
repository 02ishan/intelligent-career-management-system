import { Routes } from '@angular/router';
import { authGuard } from './guards/auth-guard';
import { LoginPage } from './pages/login-page/login-page';
import { RegisterPage } from './pages/register-page/register-page';
import { Dashboard } from './pages/dashboard/dashboard';
import { JobsPage } from './pages/jobs-page/jobs-page';
import { CreateJobPage } from './pages/create-job-page/create-job-page';
import { EditJobPage } from './pages/edit-job-page/edit-job-page';
import { MyApplicationsPage } from './pages/my-applications-page/my-applications-page';
import { ApplicationsPage } from './pages/applications-page/applications-page';

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
  },
  {
    path: 'applications',
    component: ApplicationsPage,
    canActivate: [authGuard]
  },
  {
    path: 'my-applications',
    component: MyApplicationsPage,
    canActivate: [authGuard]
  },
  {
    path: 'create-job',
    component: CreateJobPage,
    canActivate: [authGuard]
  },
  {
    path: 'edit-job/:id',
    component: EditJobPage,
    canActivate: [authGuard]
  }
];
