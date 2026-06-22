import { Injectable } from '@angular/core';
import { Item } from '../../shared/models/item.model';
import { HttpClient, HttpEvent, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { blob } from 'stream/consumers';

@Injectable({
  providedIn: 'root'
})
export class ItemService {
  url = `${apiUrl}/items`;

  constructor(private http: HttpClient) { }

  getItems(): Observable<Item[]> {
    return this.http.get<Item[]>(this.url);
  }

  createItem(item: Item) {
    return this.http.post<Item>(`${this.url}/create-item`, item, { withCredentials: true });
  }

  search(items: item[], text: string) {
    if(!text) return [];

    return items.filter(item => 
        item.itemCode.toLowerCase.includes(text.toLowerCase().trim()) ||
        item.enName.toLowerCase.includes(text.toLowerCase().trim()) ||
        item.vnName.toLowerCase.includes(text.toLowerCase().trim()) ||
        item.maker.toLowerCase.includes(text.toLowerCase().trim()) ||
        item.positionIn.toLowerCase().includes(text.toLowerCase().trim()) ||
        item.deptName.toLowerCase().includes(text.toLowerCase().trim())
    );
  }

  update(data: Item) {
    return this.http.put<Item>(`${this.url}/update-item`, data, { withCredentials: true });
  }

  deleteItem(id: UUID) {
    return this.http.put<any>(`${this.url}/delete-item/${id}`, { withCredentials: true });
  }

  updateImage(file: any, id: UUID) {
    return this.http.post<any>(`${this.url}/change-image/${id}`, file, { withCredentials: true });
  }

  exportFile() {
    return this.http.get(`${this.url}/export-item`, { responseType: 'blob' });
  }

  createListItem(file: File) {
    const formData = new FormData();
    formData.append('file', file, file.name)
    ;

    return this.http.post(`${this.url}/create-list-item`, formData,
      {
        reportProgress: true,
        observe: 'events'
      });
  }

  receiveItem(item: Item, qty: number) {
    let data = {
      itemId: item.id,
      empCode: null,
      empName: null,
      qty: qty,
      reason: null
    };

    return this.http.put<any>(`${this.url}/receive-item`, data, { withCredentials: true });
  }

  deliveryItem(data: any) {
    return this.http.put<any>(`${this.url}/delivery-item`, data, { withCredentials: true })
  }
}
