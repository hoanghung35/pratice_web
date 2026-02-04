import { Component, Input } from '@angular/core';
import { TableColumn } from '../models/table-column.model';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html'
})
export class TableComponent<T> {
  @Input() columns: TableColumn<T>[] = [];
  @Input() data: T[] = [];
}
