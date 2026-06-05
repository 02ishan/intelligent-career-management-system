import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

import { DashboardService } from '../../services/dashboard';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard implements OnInit {

  totalJobs = 0;
  totalApplications = 0;
  shortlistedCandidates = 0;
  rejectedCandidates = 0;
  selectedCandidates = 0;

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

  }

  logout() {

    localStorage.removeItem('token');

    this.router.navigate(['/login']);

  }

}
