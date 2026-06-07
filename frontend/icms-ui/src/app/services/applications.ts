import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ApplicationsService {

  private apiUrl = 'https://localhost:7187/api/Applications';

  constructor(private http: HttpClient) {
  }

  getApplications() {
    return this.http.get(this.apiUrl);
  }

  getShortlistedApplications() {
    return this.http.get(
      `${this.apiUrl}/shortlisted`
    );
  }

  getSelectedApplications() {
    return this.http.get(
      `${this.apiUrl}/selected`
    );
  }

  getRejectedApplications() {
    return this.http.get(
      `${this.apiUrl}/rejected`
    );
  }

  applyForJob(jobId: number) {
    return this.http.post(
      this.apiUrl,
      {
        jobId: jobId
      }
    );
  }

  updateStatus(
    applicationId: number,
    status: number,
    remarks: string = ''
  ) {
    return this.http.put(
      `${this.apiUrl}/${applicationId}/status`,
      {
        status,
        remarks
      }
    );
  }

}
