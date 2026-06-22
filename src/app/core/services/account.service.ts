import  { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { apiUrl } from '../constant/app.constant';
import { Account } from '../../shared/models/account.model';
import { UUID } from 'crypto';

@Injectable({
  provideIn: 'root"
})

export class AccountService {
  url = `${apiUrl}/account`;

  constructor(private http: HttpClient){}

  getAccount(): Observable<Account[]> {
    return this.http.get<Account>(this.url);
  }

  createAccount(data: any) {
    return this.http.post<any>(`${this.url}/create-account`, data, { withCredentials: true });
  }

  deleteAccount(id : UUID){
    return this.http.delete(`${this.url}/delete-account/${id}`, { withCredentials: true });
  }

  resetAccount(id: UUID) {
    return this.http.put(`${this.url}/reset-account/${id}`, { withCredentials: true });
  }

  changeProfile(data: any) {
    return this.http.put(`${this.url}/change-profile`, data, { withCredentials: true });
  }
}
