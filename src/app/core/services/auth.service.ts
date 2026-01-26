import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, tap } from 'rxjs';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private userSubject = new BehaviorSubject<any>(null);
  private me$ = new BehaviorSubject<any | null>(null);

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
    return this.http.post('/api/auth/logout', {}).pipe(
      tap(() => this.clearMe())
    );
  }

  refreshToken() {
    return this.http.post('/api/auth/refresh', {}, {
      withCredentials: true
    });
  }

  get role(): string | null {
    return this.userSubject.value?.role ?? null;
  }

  getMe(force = false): Observable<any> {
    if (!force && this.me$.value) {
      return of(this.me$.value);
    }

    return this.http.get<any>('/api/auth/me').pipe(
      tap(user => this.me$.next(user))
    );
  }

  clearMe() {
    this.me$.next(null);
  }
}
