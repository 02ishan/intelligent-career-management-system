import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Navbar } from '../../components/navbar/navbar';
import { ApplicationsService } from '../../services/applications';
import { JobsService } from '../../services/jobs';
import Swal from 'sweetalert2';

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

  appliedApplications: any[] = [];

  role = '';

  currentUserId = 0;

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

      this.currentUserId = Number(
        payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier']
      );

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

    if (this.role === 'Student') {

      this.applicationsService
        .getApplications()
        .subscribe({
          next: (response: any) => {

            this.appliedApplications = response;

            this.cdr.detectChanges();

          }
        });

    }

  }

  deleteJob(id: number) {

    Swal.fire({
      title: 'Delete Job?',
      text: 'This action cannot be undone.',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Delete',
      cancelButtonText: 'Cancel'
    })
      .then(result => {

        if (!result.isConfirmed) {
          return;
        }

        this.jobsService.deleteJob(id)
          .subscribe({
            next: (response: any) => {

              Swal.fire(
                'Deleted!',
                response.message,
                'success'
              );

              this.jobs = this.jobs.filter(
                job => job.id !== id
              );

              this.cdr.detectChanges();

            },
            error: () => {

              Swal.fire(
                'Error',
                'Failed To Delete Job',
                'error'
              );

            }
          });

      });

  }

  apply(jobId: number) {

    this.applicationsService
      .applyForJob(jobId)
      .subscribe({
        next: () => {

          Swal.fire(
            'Success',
            'Application Submitted Successfully',
            'success'
          );

          this.applicationsService
            .getApplications()
            .subscribe({
              next: (response: any) => {

                this.appliedApplications = response;

                this.cdr.detectChanges();

              }
            });

        },
        error: (error: any) => {

          if (error.status === 400) {

            Swal.fire(
              'Already Applied',
              'You have already applied for this job.',
              'info'
            );

          }
          else {

            Swal.fire(
              'Error',
              'Failed To Apply',
              'error'
            );

          }

        }
      });

  }

  hasApplied(jobTitle: string): boolean {

    return this.appliedApplications.some(
      application => application.jobTitle === jobTitle
    );

  }

  getApplicationStatus(jobTitle: string): string {

    const application =
      this.appliedApplications.find(
        application => application.jobTitle === jobTitle
      );

    return application
      ? application.status
      : '';

  }

  getStatusClass(jobTitle: string): string {

    const status = this.getApplicationStatus(jobTitle);

    switch (status) {

      case 'Applied':
        return 'bg-primary';

      case 'UnderReview':
        return 'bg-secondary';

      case 'Shortlisted':
        return 'bg-warning text-dark';

      case 'Interview':
        return 'bg-info text-dark';

      case 'Selected':
        return 'bg-success';

      case 'Rejected':
        return 'bg-danger';

      default:
        return 'bg-secondary';

    }

  }

}
