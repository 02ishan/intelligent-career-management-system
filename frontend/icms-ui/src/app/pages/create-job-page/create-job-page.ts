import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { JobsService } from '../../services/jobs';
import { Navbar } from '../../components/navbar/navbar';

@Component({
  selector: 'app-create-job-page',
  imports: [FormsModule, Navbar],
  templateUrl: './create-job-page.html',
  styleUrl: './create-job-page.css',
})
export class CreateJobPage {

  title = '';
  companyName = '';
  location = '';
  description = '';

  constructor(
    private jobsService: JobsService,
    private router: Router
  ) {
  }

  createJob() {

    const jobData = {
      title: this.title,
      companyName: this.companyName,
      location: this.location,
      description: this.description
    };

    this.jobsService.createJob(jobData)
      .subscribe({
        next: () => {

          alert('Job Created Successfully');

          this.router.navigate(['/jobs']);

        },
        error: () => {

          alert('Failed To Create Job');

        }
      });

  }

}
