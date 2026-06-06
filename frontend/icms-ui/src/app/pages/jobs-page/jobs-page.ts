import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { JobsService } from '../../services/jobs';

@Component({
  selector: 'app-jobs-page',
  imports: [
    CommonModule,
    RouterLink
  ],
  templateUrl: './jobs-page.html',
  styleUrl: './jobs-page.css',
})
export class JobsPage implements OnInit {

  jobs: any[] = [];

  constructor(
    private jobsService: JobsService,
    private cdr: ChangeDetectorRef
  ) {
  }

  ngOnInit(): void {

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
      error: (error) => {

        alert('Failed To Delete Job');

      }
    });

}

}
