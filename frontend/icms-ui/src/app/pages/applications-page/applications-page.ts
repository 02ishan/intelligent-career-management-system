import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute } from '@angular/router';
import { ApplicationsService } from '../../services/applications';
import { Navbar } from '../../components/navbar/navbar';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-applications-page',
  imports: [CommonModule, Navbar],
  templateUrl: './applications-page.html',
  styleUrl: './applications-page.css',
})
export class ApplicationsPage implements OnInit {

  applications: any[] = [];

  constructor(
    private applicationsService: ApplicationsService,
    private router: Router,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef
  ) {
  }

  ngOnInit(): void {

    this.route.queryParams.subscribe(params => {

      const filter = params['filter'];

      if (filter === 'shortlisted') {

        this.loadShortlistedApplications();

      }
      else if (filter === 'selected') {

        this.loadSelectedApplications();

      }
      else if (filter === 'rejected') {

        this.loadRejectedApplications();

      }
      else {

        this.loadApplications();

      }

    });

  }

  loadApplications() {

    this.applicationsService
      .getApplications()
      .subscribe({
        next: (response: any) => {

          this.applications = response;

          this.cdr.detectChanges();

        }
      });

  }

  loadShortlistedApplications() {

    this.applicationsService
      .getShortlistedApplications()
      .subscribe({
        next: (response: any) => {

          this.applications = response;

          this.cdr.detectChanges();

        }
      });

  }

  loadSelectedApplications() {

    this.applicationsService
      .getSelectedApplications()
      .subscribe({
        next: (response: any) => {

          this.applications = response;

          this.cdr.detectChanges();

        }
      });

  }

  loadRejectedApplications() {

    this.applicationsService
      .getRejectedApplications()
      .subscribe({
        next: (response: any) => {

          this.applications = response;

          this.cdr.detectChanges();

        }
      });

  }

  updateStatus(
    applicationId: number,
    status: number
  ) {

    this.applicationsService
      .updateStatus(
        applicationId,
        status
      )
      .subscribe({
        next: () => {

          Swal.fire(
  'Success',
  'Status Updated Successfully',
  'success'
);

          this.loadApplications();

        },
        error: () => {

          Swal.fire(
  'Error',
  'Failed To Update Status',
  'error'
);

        }
      });

  }

}
