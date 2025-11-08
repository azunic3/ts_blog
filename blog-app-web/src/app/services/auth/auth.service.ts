import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment.staging';
import { jwtDecode } from 'jwt-decode';
import { DecodedToken } from '../../models/token.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  login(username: string, password: string): Observable<any> {
    return this.http
      .post<any>(`${this.apiUrl}/Auth/Login`, { username, password })
      .pipe(
        tap((response: any) => {
          localStorage.setItem('jwtToken', response.token);
        })
      );
  }

  register(username: string, email: string, password: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/Auth/Register`, {
      username,
      email,
      password,
    });
  }

  isLoggedIn(): boolean {
    return this.isAuthenticated();
  }

  isAuthenticated(): boolean {
    return !!localStorage.getItem('jwtToken');
  }

  logout(): void {
    localStorage.removeItem('jwtToken');
  }

  getToken(): string | null {
    return localStorage.getItem('jwtToken');
  }

  isAdmin(): boolean {
    const token = this.getToken();
    if (token) {
      const decodedToken = jwtDecode<any>(token);

      const roles =
        decodedToken[
          'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
        ];
      return Array.isArray(roles) ? roles.includes('Admin') : roles === 'Admin';
    }

    return false;
  }
}
