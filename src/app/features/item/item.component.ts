import { Component, OnInit } from '@angular/core';
import { ItemService } from '../../core/services/item.service';
import { Item } from '../../shared/models/item.model';
import { TableColumn } from '../../shared/models/table-column.model';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-item-list',
  templateUrl: './item-list.component.html',
  providers: [DatePipe]
})
export class ItemListComponent implements OnInit {
  items: Item[] = [];


  columns: TableColumn<Item>[] = [
    { key: 'enName', label: 'EN Name' },
    { key: 'vnName', label: 'VN Name' },
    { key: 'maker', label: 'Maker' },
    {
      key: 'quantity',
      label: 'Qty',
      render: i => i.quantity.toString()
    },
    { key: 'position', label: 'Position' },
    {
      key: 'dateEntry',
      label: 'Date Entry',
      render: i => this.datePipe.transform(i.dateEntry, 'dd/MM/yyyy')!
    }
  ];

  constructor(
    private itemService: ItemService,
    private datePipe: DatePipe
  ) { }

  ngOnInit() {

    this.itemService.getItems().subscribe((res: Item[]) => {
      this.items = res;
    });
  }
}
