import { Component, OnInit, ViewChild, TemplateRef, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { ItemService } from '../../core/services/item.service';
import { DialogService } from '../../core/services/dialog.service';
import { Item } from '../../shared/models/item.model';
import { TableColumn } from '../../shared/models/table-column.model';
import { DatePipe } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { CreateItemComponent } from './item.dialog/create.item/create.item.component';

@Component({
  selector: 'app-item',
  standalone: true,
  templateUrl: './item.component.html',
  styleUrl: './item.component.scss',
  imports: [SharedModule',
  providers: [DatePipe]
})
export class ItemComponent implements OnInit, AfterViewInit {
  items: Item[] = [];
  dataBackup: Item[] = [];
  searchText = "";
  canDelete: boolean = false;
  canAction: boolean = false;
  canChangeInOutItem: boolean = false;

  @ViewChild('action', { static: true })
  actionTpl!: TemplateRef<any>;
  selected?: string;

  columns: TableColumn<Item>[] = [];

  ngAfterViewInit(): void {
    this.columns = [
      { key: 'enName', label: 'EN Name', width: '31%' },
      { key: 'vnName', label: 'VN Name', width: '31%' },
      { key: 'maker', label: 'Maker', width: '7%', style: 'center' },
      {
        key: 'quantity',
        label: 'Qty',
        width: '6%',
        render: i => i.quantity.toString()
      },
      { key: 'positionIn', label: 'Position', width: '7%', style: 'center' },
      {
        key: 'deptName',
        label: 'Location',
        width: '7%',
        style: 'center'
      },
      { label: 'Actions', template: this.actionTpl }
    ];

    this.cdRef.detectChanges();
  }

  constructor(
    private itemService: ItemService,
    private dialogService: DialogService,
    private dialog: MatDialog,
    private cdRef: ChangeDetectorRef,
    private authService: AuthService
  ) { }

  ngOnInit() {
    this.getAllItem();
    this.canAction = this.authService.role == 'dev' || this.authService.role == 'super';
    this.canChangeInOutItem = this.authService.role == 'dev' || this.authService.role == 'admin';
  }

  edit(id: UUID) {
    const item = this.items.find(i => i.id === id);
    if (!item) return;

    this.dialog.open(ItemEditComponent, {
      data: item,
      maxWidth: '80rem',
      width: '60rem',
      panelClass: 'item-edit-container',
      disableClose: true
    })
      .afterClosed().subscribe(() => {
        this.getAllItem();
      });
  }

  delete(id: UUID) {
      if(window.confirm("Delete item?")) {
        this.itemService.deleteItem(id).subscribe({
          next: () => {
            this.dialogService.AutoAlterDialog({
              message: 'Delete Item Success',
              showcheck: true
            });
          },
          error: (err) => {
            this.dialogService.AutoAlterDialog({
              message: 'Delete Item Faild!',
              showcheck: false
            });
          }
        });
      }
  }

  subDetails(id: UUID) {
    const item = this.items.find(i => i.id === id);
    this.dialog.open(ItemDialogComponent, {
      data: item,
      maxWidth: '80rem',
      width: '50rem',
      panleClass: 'item-details-container',
      disableClose: true
    })
      .afterClosed().subscribe(() => {
        this.getAllItem();
      });
  }

  getData(text: string) {
    if(!text) {
      this.items = this.dataBackup;
      return;
    }

    this.items = this.dataBackup;
    ths.items = this.itemService.search(this.items, text);
  }

  getAllItem() {
    this.itemService.getItem().subsribe((res: Item[]) => {
      this.items = res;
      this.dataBackup = res;
      this.cdRef.detectChanges();
    });
  }

  addItem() {
    this.dialog.open(ItemCreateComponent, {
      maxWidth: '80rem',
      width: '60rem',
      panelClass: 'item-create-container',
      disableClose: true
    })
      .afterClosed().subscribe(() => {
        this.getAllItem();
      });
  }

  exportItem() {
    this.itemService.exportFile().subscribe(blob => {
      saveAs(blob, `${formatDate(Date.Now(), 'yyyyMMdd', 'em-US')}item.xlsx`)
    });
  }

  uploadItem() {
    this.dialog.open(UploadFileItemComponent, {
      maxWidth: '60rem',
      width: '40rem',
      panelClass: 'upload-file-container',
      disableClose: true
    })
      .afterClosed().subscribe({
        next: () => this.getAllItem(),
        error: (err) => console.log(err)
      });
  }
}
