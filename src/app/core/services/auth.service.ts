import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private userSubject = new BehaviorSubject<any>(null);
  user$ = this.userSubject.asObservable();

  constructor(private http: HttpClient) { }

  login(username: string, password: string) {
    return this.http.post<any>(
      '/api/auth/login',
      { username, password },
      { withCredentials: true }
    ).pipe(
      tap(user => this.userSubject.next(user))
    );
  }

  logout() {
    return this.http.post('/api/auth/logout', {}, { withCredentials: true })
      .pipe(tap(() => this.userSubject.next(null)));
  }

  refreshToken() {
    return this.http.post('/api/auth/refresh', {}, {
      withCredentials: true
    });
  }

  get role(): string | null {
    return this.userSubject.value?.role ?? null;
  }

  isLoggedIn(): boolean {
    return !!this.userSubject.value;
  }
}
