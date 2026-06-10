import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Navbar } from '../../components/navbar/navbar';
import { DashboardService } from '../../services/dashboard';

@Component({
  selector: 'app-dashboard',
  imports: [
    CommonModule,
    Navbar
  ],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard implements OnInit {

  totalJobs = 0;
  totalApplications = 0;
  shortlistedCandidates = 0;
  rejectedCandidates = 0;
  selectedCandidates = 0;
  role = '';

  constructor(
    private router: Router,
    private dashboardService: DashboardService,
    private cdr: ChangeDetectorRef
  ) {
  }

  ngOnInit(): void {

    this.dashboardService.getStats()
      .subscribe({
        next: (response: any) => {

          this.totalJobs = response.totalJobs;
          this.totalApplications = response.totalApplications;
          this.shortlistedCandidates = response.shortlistedCandidates;
          this.rejectedCandidates = response.rejectedCandidates;
          this.selectedCandidates = response.selectedCandidates;

          this.cdr.detectChanges();

        },
        error: () => {
        }
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

  navigateToJobs() {
    this.router.navigate(['/jobs']);
  }

  navigateToApplications() {
    this.router.navigate(['/applications']);
  }

  navigateToShortlisted() {
    this.router.navigate(
      ['/applications'],
      {
        queryParams: {
          filter: 'shortlisted'
        }
      }
    );
  }

  navigateToSelected() {
    this.router.navigate(
      ['/applications'],
      {
        queryParams: {
          filter: 'selected'
        }
      }
    );
  }

  navigateToRejected() {
    this.router.navigate(
      ['/applications'],
      {
        queryParams: {
          filter: 'rejected'
        }
      }
    );
  }

}
