import { Component, OnInit, ViewChild, TemplateRef, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { ItemService } from '../../core/services/item.service';
import { Item } from '../../shared/models/item.model';
import { TableColumn } from '../../shared/models/table-column.model';
import { DatePipe } from '@angular/common';
import { SharedModule } from '../../shared/shared.module';

@Component({
  selector: 'app-item',
  standalone: false,
  templateUrl: './item.component.html',
  providers: [DatePipe]
})
export class ItemComponent implements OnInit, AfterViewInit {
  items: Item[] = [];

  @ViewChild('action', { static: true })
  actionTpl!: TemplateRef<any>;

  columns: TableColumn<Item>[] = [];

  ngAfterViewInit() {
    // build columns after actionTpl is available
    this.columns = [
      { key: 'enName', label: 'EN Name' },
      { key: 'vnName', label: 'VN Name' },
      { key: 'maker', label: 'Maker' },
      {
        key: 'quantity',
        label: 'Qty',
        render: i => i.quantity.toString()
      },
      { key: 'positionIn', label: 'Position' },
      { label: 'Actions', template: this.actionTpl }
    ];

    // avoid ExpressionChangedAfterItHasBeenCheckedError
    this.cdRef.detectChanges();
  }

  constructor(
    private itemService: ItemService,
    private datePipe: DatePipe,
    private cdRef: ChangeDetectorRef
  ) { }

  ngOnInit() {
    this.itemService.getItems().subscribe((res: Item[]) => {
      this.items = res;
      // if service emits synchronously, notify change detector
      this.cdRef.detectChanges();
    });
  }

  edit(id: string) {

  }

  delete(id: string) {

  }
}
