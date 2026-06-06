import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChangeDetectorRef } from '@angular/core';

import { JobsService } from '../../services/jobs';

@Component({
  selector: 'app-jobs-page',
  imports: [CommonModule],
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

          console.log(response);

          this.jobs = response;

          this.cdr.detectChanges();

        },
        error: () => {
        }
      });

  }

}
