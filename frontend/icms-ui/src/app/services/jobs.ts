import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class JobsService {

  private apiUrl = 'https://localhost:7187/api/Jobs';

  constructor(private http: HttpClient) {
  }

  getJobs() {
    return this.http.get(this.apiUrl);
  }
  createJob(jobData: any) {
    return this.http.post(
      this.apiUrl,
      jobData
    );
  }
  updateJob(id: number, jobData: any) {
    return this.http.put(
      `${this.apiUrl}/${id}`,
      jobData
    );
  }
  getJobById(id: number) {
    return this.http.get(
      `${this.apiUrl}/${id}`
    );
  }
  deleteJob(id: number) {
    return this.http.delete(
      `${this.apiUrl}/${id}`
    );
  }

}
