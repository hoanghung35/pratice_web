import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { TableColumn } from '../models/table-column.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-table',
  standalone: true,
  templateUrl: './table.component.html',
  imports: [CommonModule, FormsModule]
})
export class TableComponent<T> implements OnChanges {
  @Input() columns: TableColumn<T>[] = [];
  @Input() data: T[] = [];

  // pagination inputs
  @Input() pageSizeOptions: number[] = [5, 10, 20];
  @Input() pageSize = 10;

  // internal state
  currentPage = 0;

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['data']) {
      this.currentPage = 0;
    }
  }

  /** return only the items for the current page */
  get paginatedData(): T[] {
    const start = this.currentPage * this.pageSize;
    return this.data.slice(start, start + this.pageSize);
  }

  get totalPages(): number {
    return Math.ceil(this.data.length / this.pageSize);
  }

  prevPage(): void {
    if (this.currentPage > 0) {
      this.currentPage--;
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages - 1) {
      this.currentPage++;
    }
  }

  pageSizeChanged(newSize: number): void {
    this.pageSize = newSize;
    this.currentPage = 0; // reset to first page when size changes
  }
}
