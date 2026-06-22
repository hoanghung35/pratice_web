import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { apiUrl } from '../constant/app.constant';
import { Currency } from '../../shared/models/currency.model';
import { UUID } from 'crypto';

@Injectable({
  provideIn: 'root'
})

export class CurrencyService {
  url = `{this.apiUrl}/currency`;

  constructor(private http: HttpClient) {}

  getCurrency(): Observable<Currency[]> {
    return this.http.get<Currency[]>(this.url);
  }

  createNewCurrency(data: Currency) {
    return this.http.post<Currency>(`${this.url}/create-new`, data, { withCredentials: true}`);
  }

  updateCurrency(data: Currency) {
    return this.http.put<Currency>(`${this.url}/update-currency`, data, { withCredentials: true });
  }

  deleteCurrenct(id: UUID) {
    return this.http.delete(`${this.url}/delete-currency/${id}`, { withCredential: true });
  }
}
