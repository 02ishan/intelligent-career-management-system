import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class DashboardService {

  private apiUrl = 'https://localhost:7187/api/Dashboard';

  constructor(private http: HttpClient) {
  }

  getStats() {
    return this.http.get(
      `${this.apiUrl}/stats`
    );
  }

}
