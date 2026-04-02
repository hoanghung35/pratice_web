import { Injectable } from '@angular/core';
import { Item } from '../../shared/models/item.model';
import { HttpClient, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { blob } from 'stream/consumers';

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

  importFile(file: File): Observable<HttpEvent<any>> {
    const formData = new FormData();
    formData.append('file', file, file.name);

    return this.http.post('http://localhost:5251/api/items/import-file', formData, {
      reportProgress: true,
      observe: 'events'
    });
  }

  exportItem() {
    return this.http.get('http://localhost:5251/api/export-item', { responseType: 'blob' }).subscribe(blob => {
      //saveAs(blob, 'report.xlsx');
    })
  }
}
