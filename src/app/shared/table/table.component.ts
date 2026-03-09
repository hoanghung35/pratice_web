import { Component, Input } from '@angular/core';
import { TableColumn } from '../models/table-column.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-table',
  standalone: true,
  templateUrl: './table.component.html',
  imports: [CommonModule]
})
export class TableComponent<T> {
  @Input() columns: TableColumn<T>[] = [];
  @Input() data: T[] = [];
}
