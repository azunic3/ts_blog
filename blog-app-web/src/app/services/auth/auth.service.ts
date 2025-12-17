import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { jwtDecode } from 'jwt-decode';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  // =========================
  // LOGIN (JWT u response body)
  // =========================
  login(username: string, password: string): Observable<any> {
    return this.http
      .post<any>(
        `${this.apiUrl}/auth/login`,
        { username, password }
        // ⛔ withCredentials ISKLJUČENO (nema cookies)
      )
      .pipe(
        tap((response: any) => {
          const token = response.token;
          this.storeToken(token);
        })
      );
  }

  // =========================
  // LOGOUT (frontend-only)
  // =========================
  logout(): void {
    // ⛔ backend više NE briše cookies
    // ⛔ nema auth/logout requesta
    localStorage.removeItem('jwtToken');
  }

  // =========================
  // REGISTER
  // =========================
  register(data: any): Observable<any> {
    return this.http.post(
      `${this.apiUrl}/auth/register`,
      data
      // ⛔ withCredentials ISKLJUČENO
    );
  }

  // =========================
  // TOKEN HELPERS
  // =========================
  getToken(): string | null {
    return localStorage.getItem('jwtToken');
  }

  isAuthenticated(): boolean {
    const token = this.getToken();
    if (!token) return false;
    return !this.isTokenExpired(token);
  }

  isLoggedIn(): boolean {
    return this.isAuthenticated();
  }

  isAdmin(): boolean {
    const token = this.getToken();
    if (!token || this.isTokenExpired(token)) return false;

    const decoded: any = jwtDecode(token);
    const roles =
      decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

    return Array.isArray(roles)
      ? roles.includes('Admin')
      : roles === 'Admin';
  }

  private isTokenExpired(token: string): boolean {
    const decoded: any = jwtDecode(token);

    if (!decoded.exp) return true;

    const now = Date.now() / 1000;
    return decoded.exp < now;
  }

  // =========================
  // JWT STORAGE (LOCALSTORAGE)
  // =========================
  private storeToken(token: string) {
    localStorage.setItem('jwtToken', token);
  }

  // =========================
  // ⛔ COOKIE-BASED LOGIKA (ISKLUČENA)
  // =========================

  /*
  private storeTokenWithRealExpiration(token: string) {
    const decoded: any = jwtDecode(token);
    const exp = decoded.exp * 1000;
    const expirationDate = new Date(exp);
    document.cookie =
      `jwtToken=${token}; expires=${expirationDate.toUTCString()}; path=/; SameSite=None; Secure`;
  }

  private getCookie(name: string): string | null {
    const cookies = document.cookie.split('; ');
    const found = cookies.find((row) => row.startsWith(name + '='));
    return found ? found.split('=')[1] : null;
  }

  private deleteCookie(name: string) {
    document.cookie =
      `${name}=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/; SameSite=None; Secure`;
  }

  refresh(): Observable<any> {
    return this.http.post<any>(
      `${this.apiUrl}/auth/refresh`,
      {},
      { withCredentials: true }
    );
  }

  saveAccessToken(token: string) {
    const decoded: any = jwtDecode(token);
    const exp = decoded.exp * 1000;
    const expires = new Date(exp).toUTCString();
    document.cookie =
      `jwtToken=${token}; expires=${expires}; path=/; SameSite=None; Secure`;
  }
  */
}
