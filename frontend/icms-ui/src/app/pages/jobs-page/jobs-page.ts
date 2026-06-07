import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Navbar } from '../../components/navbar/navbar';
import { ApplicationsService } from '../../services/applications';
import { JobsService } from '../../services/jobs';

@Component({
  selector: 'app-jobs-page',
  imports: [
    CommonModule,
    RouterLink,
    Navbar
  ],
  templateUrl: './jobs-page.html',
  styleUrl: './jobs-page.css',
})
export class JobsPage implements OnInit {

  jobs: any[] = [];

  role = '';

  constructor(
    private jobsService: JobsService,
    private applicationsService: ApplicationsService,
    private cdr: ChangeDetectorRef
  ) {
  }

  ngOnInit(): void {

    const token = localStorage.getItem('token');

    if (token) {

      const payload = JSON.parse(
        atob(token.split('.')[1])
      );

      this.role =
        payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
    }

    this.jobsService.getJobs()
      .subscribe({
        next: (response: any) => {

          this.jobs = response;

          this.cdr.detectChanges();

        },
        error: () => {
        }
      });

  }

  deleteJob(id: number) {

    if (!confirm('Are you sure you want to delete this job?')) {
      return;
    }

    this.jobsService.deleteJob(id)
      .subscribe({
        next: (response: any) => {

          alert(response.message);

          this.jobs = this.jobs.filter(
            job => job.id !== id
          );

          this.cdr.detectChanges();

        },
        error: () => {

          alert('Failed To Delete Job');

        }
      });

  }

  apply(jobId: number) {

    this.applicationsService
      .applyForJob(jobId)
      .subscribe({
        next: () => {

          alert('Application Submitted Successfully');

        },
        error: (error: any) => {

          if (error.status === 400) {

            alert('You have already applied for this job.');

          }
          else {

            alert('Failed To Apply');

          }

        }
      });

  }

}
