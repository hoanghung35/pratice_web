import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, tap } from 'rxjs';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {

  private me$ = new BehaviorSubject<any | null>(null);

  constructor(private http: HttpClient) { }

  login(username: string, password: string) {
    return this.http.post(
      '/api/auth/login',
      { username, password },
      { withCredentials: true }
    ).pipe(
      tap(() => this.clearMe())
    );
  }

  logout() {
    return this.http.post(
      '/api/auth/logout',
      {},
      { withCredentials: true }
    ).pipe(
      tap(() => this.clearMe())
    );
  }

  refreshToken() {
    return this.http.post(
      '/api/auth/refresh',
      {},
      { withCredentials: true }
    );
  }

  getMe(force = false): Observable<any> {
    if (!force && this.me$.value) {
      return of(this.me$.value);
    }

    return this.http.get<any>(
      '/api/auth/me',
      { withCredentials: true }
    ).pipe(
      tap(user => this.me$.next(user))
    );
  }

  clearMe() {
    this.me$.next(null);
  }

  get user$() {
    return this.me$.asObservable();
  }

  get role(): string | null {
    return this.me$.value?.role ?? null;
  }
}
