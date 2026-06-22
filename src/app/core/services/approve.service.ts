import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Approve } from '../shared/models/approve.model';
import { apiUrl } from '../constant/app.constant';
import { UUID } from 'crypto';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApproveService {
  url = `${apiUrl}/approve-request`;
  
  constructor(private http: HttpClient) { }

  getApprove(){
    return this.http.get<Approve[]>(this.url);
  }

  approve(id: UUID) {
    return this.http.post<any>(`${this.url}/approve/${id}`, { withCredentials: true });
  }

  reject(id: UUID) {
    return this.http.post<any>(`${this.url}`/reject/${id});
  }

  approveAll(data: any[]): Observable<any> {
    return this.http.put<any>(`${this.url}/approve/all`, data, { withCredentials: true });
  }
}
