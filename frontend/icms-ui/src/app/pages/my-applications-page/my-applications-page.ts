import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Navbar } from '../../components/navbar/navbar';
import { ApplicationsService } from '../../services/applications';

@Component({
  selector: 'app-my-applications-page',
  imports: [CommonModule, Navbar],
  templateUrl: './my-applications-page.html',
  styleUrl: './my-applications-page.css',
})
export class MyApplicationsPage implements OnInit {

  applications: any[] = [];

  constructor(
    private applicationsService: ApplicationsService,
    private cdr: ChangeDetectorRef
  ) {
  }

  ngOnInit(): void {

    this.applicationsService
      .getApplications()
      .subscribe({
        next: (response: any) => {

          this.applications = response;

          this.cdr.detectChanges();

        },
        error: () => {
        }
      });

  }

}
