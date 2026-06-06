import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ChangeDetectorRef } from '@angular/core';
import { JobsService } from '../../services/jobs';

@Component({
  selector: 'app-edit-job-page',
  imports: [FormsModule],
  templateUrl: './edit-job-page.html',
  styleUrl: './edit-job-page.css',
})
export class EditJobPage implements OnInit {

  id = 0;

  title = '';
  companyName = '';
  location = '';
  description = '';

constructor(
  private route: ActivatedRoute,
  private router: Router,
  private jobsService: JobsService,
  private cdr: ChangeDetectorRef
) {
}

  ngOnInit(): void {

    this.id = Number(
      this.route.snapshot.paramMap.get('id')
    );

    this.jobsService.getJobById(this.id)
      .subscribe({
        next: (response: any) => {

          console.log(response);

          this.title = response.title;
          this.companyName = response.companyName;
          this.location = response.location;
          this.description = response.description;
          this.cdr.detectChanges();
        },
        error: (error) => {
          console.log(error);
        }
      });

  }

  updateJob() {

    const jobData = {
      title: this.title,
      companyName: this.companyName,
      location: this.location,
      description: this.description
    };

    this.jobsService.updateJob(
      this.id,
      jobData
    ).subscribe({
      next: () => {

        alert('Job Updated Successfully');

        this.router.navigate(['/jobs']);

      },
      error: () => {

        alert('Failed To Update Job');

      }
    });

  }

}
