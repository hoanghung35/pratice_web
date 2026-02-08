import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, tap } from 'rxjs';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {

  private me$ = new BehaviorSubject<any | null>(null);
  private _url = "";

  constructor(private http: HttpClient) { }

  login(username: string, password: string) {
    return this.http.post(
      `${this._url}/login`,
      { username, password },
      { withCredentials: true }
    ).pipe(
      tap(() => this.clearMe())
    );
  }

  logout() {
    return this.http.post(
      `${this._url}/logout`,
      {},
      { withCredentials: true }
    ).pipe(
      tap(() => this.clearMe())
    );
  }

  refreshToken() {
    return this.http.post(
      `${this._url}/refresh`,
      {},
      { withCredentials: true }
    );
  }

  getMe(force = false): Observable<any> {
    if (!force && this.me$.value) {
      return of(this.me$.value);
    }

    return this.http.get<any>(
      `${this._url}/me`,
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
