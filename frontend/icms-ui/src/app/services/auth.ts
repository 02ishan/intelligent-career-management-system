import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class Auth {

  private apiUrl = 'https://localhost:7187/api/Auth';

  constructor(private http: HttpClient) { }

  login(loginData: any) {
    return this.http.post(
      `${this.apiUrl}/login`,
      loginData
    );
  }

  register(registerData: any) {
    return this.http.post(
      `${this.apiUrl}/register`,
      registerData
    );
  }

}
