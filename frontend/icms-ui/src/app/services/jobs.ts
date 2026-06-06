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

}
