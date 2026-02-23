import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, tap } from 'rxjs';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthService {

  private me$ = new BehaviorSubject<any | null>(null);
  private _url = "";
  private accessToken: string | null = null;
  private _refreshToken: string | null = null;
  private expireTime: Date | null = null;
  private claims: any = null;

  constructor(private http: HttpClient) { }

  getAccessToken(): string | null {
    return this.accessToken;
  }

  login(username: string, password: string): Observable<any> {
    return this.http.post<any>(
      `${this._url}/login`,
      { username, password }
    ).pipe(
      tap(response => {
        this.accessToken = response.access_token;
        this._refreshToken = response.refresh_token;
        this.expireTime = new Date(response.expire_time);
        this.claims = response.claims;
        this.clearMe();
      })
    );
  }

  logout(): Observable<any> {
    return this.http.post(
      `${this._url}/logout`,
      {}
    ).pipe(
      tap(() => {
        this.accessToken = null;
        this._refreshToken = null;
        this.expireTime = null;
        this.claims = null;
        this.clearMe();
      })
    );
  }

  refreshToken(): Observable<any> {
    return this.http.post<any>(
      `${this._url}/refresh`,
      { refresh_token: this.refreshToken }
    ).pipe(
      tap(response => {
        this.accessToken = response.access_token;
        this.expireTime = new Date(response.expire_time);
      })
    );
  }

  getMe(force = false): Observable<any> {
    if (!force && this.me$.value) {
      return of(this.me$.value);
    }

    if (this.claims) {
      this.me$.next(this.claims);
      return of(this.claims);
    }

    return of(null);
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
