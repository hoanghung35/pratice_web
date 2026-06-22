import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Order } from '../../shared/models/order.model';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  constructor(private http: HttpClient) { }

  getRequest(): Observable<Request[]> {
    return this.http.get<Request>(`${apiUrl}/request`);
  }
}
