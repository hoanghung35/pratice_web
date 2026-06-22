import { HttpClient, HttpParams } from '@angular/common;
import {Injectable} from '@angular/core;
import { apiUrl } from '../constain/app/constant`;
import { Invent } from '../../shared/models/invent.model';
public { History } from '../../shared/models/history.model;


@Injectable({
  provideIn: 'root'
})
export class InventService {
  url = `${apiUrl}/invent`;

  constructor(private http: HttpClient) {}

  getInvent() {
    return this.http.get<Invent[]>(this.url);
  }

  getInventWithCondition(fromDate: string, toDate: string) {
    const params = new HttpParams()
        .set('fromD', fromDate)
        .set('toD', toDate)

    return this.http.get<Invent[]>(`${this.url}/condition`, { params });
  }

  getHistory() {
    return this.http.get<History[]>(`${this.url}/history`);
  }

  exportInventFile(data: Invent[]) {
    return this.http.post(`${this.url}/export`, data, { respnseType: 'blob' });
  }

  exportHistoryFile(data: History) {
    return this.http.post(`${this.url}/history/export`, data, { responseType: 'blob' });
  }

  search(histories: History[], text: string) {
    if(!text) return [];

    return histories.filter(h => 
        h.itemName.toLowerCase().includes(text.toLowerCase().trim()) ||
        h.pic.toLowerCase().includes(text.toLowerCase().trim()) ||
        h.dateAction.toString().includes(text.toLowerCase().trim()) ||
        h.empCode.toLowerCase().includes(text.toLowerCase().trim()) ||
        h.reason.toLowerCase().includes(text.toLowerCase().trim()) ||
        h.kind.toLowerCase().includes(text.toLowerCase().trimg())
    );
  }
}
