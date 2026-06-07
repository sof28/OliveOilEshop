import { Injectable, PLATFORM_ID, inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, tap } from 'rxjs';
import { environment } from '../../environment';
import { jwtDecode } from 'jwt-decode';

interface JwtPayload {
  sub: string;
  unique_name: string;
  role: string;
}

export interface AuthUser {
  id: string;
  username: string;
  role: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly apiUrl = `${environment.apiUrl}/auth`;
  private readonly tokenKey = 'jwt_token';
  private readonly isBrowser = isPlatformBrowser(inject(PLATFORM_ID));

  private currentUserSubject = new BehaviorSubject<AuthUser | null>(this.decodeStoredToken());
  currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient) {}

  login(username: string, password: string) {
    return this.http
      .post<{ token: string }>(`${this.apiUrl}/login`, { username, password })
      .pipe(
        tap((res) => {
          if (this.isBrowser) {
            localStorage.setItem(this.tokenKey, res.token);
          }
          this.currentUserSubject.next(this.decodeStoredToken());
        }),
      );
  }

  logout() {
    if (this.isBrowser) {
      localStorage.removeItem(this.tokenKey);
    }
    this.currentUserSubject.next(null);
  }

  getToken(): string | null {
    if (!this.isBrowser) return null;
    return localStorage.getItem(this.tokenKey);
  }

  isAdmin(): boolean {
    return this.currentUserSubject.value?.role === 'Admin';
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  getCurrentUser(): AuthUser | null {
    return this.currentUserSubject.value;
  }

  private decodeStoredToken(): AuthUser | null {
    if (!this.isBrowser) return null;
    const token = localStorage.getItem(this.tokenKey);
    if (!token) return null;
    try {
      const payload = jwtDecode<JwtPayload>(token);
      return { id: payload.sub, username: payload.unique_name, role: payload.role };
    } catch {
      return null;
    }
  }
}

