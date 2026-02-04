import { Component } from '@angular/core';
import { TableColumn } from '../../shared/models/table-column.model';
import { Order } from '../../shared/models/order.model';

@Component({
  selector: 'app-order',
  standalone: false,
  templateUrl: './order.component.html',
  styleUrl: './order.component.scss'
})
export class OrderComponent {
  columns: TableColumn<Order>[] = [
    { key: 'orderNo', label: 'Order No' },
    {
      key: 'createdAt',
      label: 'Date',
      render: o => this.datePipe.transform(o.createdAt)
    }
  ];
  datePipe: any;

}
