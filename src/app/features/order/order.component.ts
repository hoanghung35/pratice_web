import { OrderService } from './../../core/services/order.service';
import { JwtCookieInterceptor } from './../../core/interceptors/jwt-cookie.interceptor';
import { AfterViewInit, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { TableColumn } from '../../shared/models/table-column.model';
import { Order } from '../../shared/models/order.model';

@Component({
  selector: 'app-order',
  standalone: false,
  templateUrl: './order.component.html',
  styleUrl: './order.component.scss'
})
export class OrderComponent implements OnInit, AfterViewInit {
  orders: Order[] = [];

  columns: TableColumn<Order>[] = [];

  constructor(
    private orderService: OrderService,
    private cdRef: ChangeDetectorRef
  ) { }

  ngAfterViewInit(): void {
    this.columns = [
      { key: 'enName', label: 'Item Name' },
      { key: 'maker', label: 'Maker' },
      { key: 'quantity', label: 'Quantity' },
      { key: 'dateOrder', label: 'Date' }
    ];
    this.cdRef.detectChanges();
  }

  ngOnInit(): void {
    this.orderService.getOrder().subscribe((res: any) => {
      this.orders = res;
      this.cdRef.detectChanges();
    })
  }

}
