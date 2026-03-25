import { Injectable } from '@angular/core';
import { Item } from '../../shared/models/item.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ItemService {


  constructor(private http: HttpClient) { }

  getItems() {
    return this.http.get<Item[]>('http://localhost:5251/api/items');
  }

  updateItem(id: string, item: Partial<Item>) {
    return this.http.put<Item>(`http://localhost:5251/api/items/${id}`, item);
  }
}
