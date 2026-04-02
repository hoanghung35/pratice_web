import { Component, OnInit, ViewChild, TemplateRef, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { ItemService } from '../../core/services/item.service';
import { DialogService } from '../../core/services/dialog.service';
import { Item } from '../../shared/models/item.model';
import { TableColumn } from '../../shared/models/table-column.model';
import { DatePipe } from '@angular/common';
import { SharedModule } from '../../shared/shared.module';
import { Observable } from 'rxjs';
import { HttpEvent } from '@angular/common/http';

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

    this.cdRef.detectChanges();
  }

  constructor(
    private itemService: ItemService,
    private dialogService: DialogService,
    private datePipe: DatePipe,
    private cdRef: ChangeDetectorRef
  ) { }

  ngOnInit() {
    this.itemService.getItems().subscribe((res: Item[]) => {
      this.items = res;

      this.cdRef.detectChanges();
    });
  }

  edit(id: string) {
    const item = this.items.find(i => i.id === id);
    if (!item) return;

    const dialogConfig: any = {
      title: 'Edit Item',
      type: 'form',
      fields: [
        { name: 'enName', label: 'EN Name', type: 'text', value: item.enName },
        { name: 'vnName', label: 'VN Name', type: 'text', value: item.vnName },
        { name: 'quantity', label: 'Quantity', type: 'number', value: item.quantity },
        { name: 'maker', label: 'Maker', type: 'text', value: item.maker },
        { name: 'positionIn', label: 'Position', type: 'text', value: item.positionIn }
      ],
      confirmText: 'Save'
    };

    this.dialogService.openCustomDialog(dialogConfig).afterClosed().subscribe((result: any) => {
      if (result) {
        console.log('Updated data:', result);
        this.itemService.updateItem(id, result).subscribe(updatedItem => {
          const index = this.items.findIndex(i => i.id === id);
          if (index !== -1) {
            this.items[index] = updatedItem;
            this.cdRef.detectChanges();
          }
        });
      }
    });
  }

  delete(id: string) {

  }

  importFile(file: File) {
    this.itemService.importFile(file).subscribe();
  }

  exportItem() {
    this.itemService.exportItem();
  }
}
