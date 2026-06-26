import { Component, Input, OnChanges, SimpleChanges, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { TableColumn } from '../models/table-column.model';
import { CommonModule } from '@angular/common';

import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';

@Component({
  selector: 'app-table',
  standalone: true,
  templateUrl: './table.component.html',
  imports: [CommonModule, MatTableModule, MatPaginatorModule]
})
export class TableComponent<T> implements OnChanges, AfterViewInit {
  @Input() columns: TableColumn<T>[] = [];
  @Input() data: T[] = [];

  @Input() pageSizeOptions: number[] = [5, 10, 20];
  @Input() pageSize = 10;

  dataSource = new MatTableDataSource<T>([]);

  displayedColumns: string[] = [];

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['data']) {
      this.dataSource.data = this.data;
      if (this.paginator) {
        this.dataSource.paginator = this.paginator;
      }
    }

    if (changes['columns']) {
      this.displayedColumns = this.columns.map((col, i) => this.getColumnId(col, i));
    }
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
  }

  getColumnId(col: TableColumn<T>, index: number): string {
    if (col.id) {
      return col.id;
    }
    if (col.key) {
      return col.key.toString();
    }
    return `col${index}`;
  }
}
