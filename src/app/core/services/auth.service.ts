import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { catchError, map, switchMap, tap } from 'rxjs/operators';

export interface UserInfo {
  id: string;
  usercode: string;
  role: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {

  private readonly _url = 'http://localhost:5251/api';

  private _user = new BehaviorSubject<UserInfo | null>(null);
  user$ = this._user.asObservable();

  constructor(private http: HttpClient) { }

  login(usercode: string, password: string): Observable<UserInfo> {
    return this.http.post<void>(
      `${this._url}/login`,
      { usercode, password },
      { withCredentials: true }
    ).pipe(
      switchMap(() => this.fetchUserInfo())
    );
  }

  logout(): Observable<void> {
    return this.http.post<void>(
      `${this._url}/logout`,
      {},
      { withCredentials: true }
    ).pipe(
      tap(() => this._user.next(null))
    );
  }

  refreshToken(): Observable<void> {
    return this.http.post<void>(
      `${this._url}/refresh`,
      {},
      { withCredentials: true }
    );
  }

  fetchUserInfo(): Observable<UserInfo> {
    return this.http.get<UserInfo>(`${this._url}/infor`, { withCredentials: true })
      .pipe(tap(u => this._user.next(u)));
  }

  getMe(force = false): Observable<UserInfo | null> {
    if (!force && this._user.value) {
      return of(this._user.value);
    }
    return this.fetchUserInfo().pipe(
      catchError(() => of(null))
    );
  }

  isAuthenticated(): Observable<boolean> {
    if (this._user.value) {
      return of(true);
    }
    return this.fetchUserInfo().pipe(
      map(u => !!u),
      catchError(() => of(false))
    );
  }

  get role(): string | null {
    return this._user.value?.role ?? null;
  }
}
