import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ItemService {
  http: any;

  constructor() { }

  getItems() {
    return this.http.get('/api/items');
  }
}
