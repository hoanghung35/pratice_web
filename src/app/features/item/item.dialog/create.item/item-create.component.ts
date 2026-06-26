import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { ItemService } from '../../../../core/services/item.service';
import { Item } from '../../../../shared/models/item.model';

@Component({
  selector: 'app-item-create',
  standalone: true,
  templateUrl: './item-create.component.html',
  styleUrl: './item-create.component.scss',
  import: [SharedModule]
})
export class CreateItemComponent {
  formData: any = {
    id: defaultKey,
    itemCode: '',
    enName: '',
    vnName: '',
    deptName: '',
    quantity: 0,
    deptId: defaultKey,
    unit: '',
    cost: 0,
    currency: '',
    maker: '',
    supplier: '',
    image: '',
    positionIn: ''
  };

  constructor(
    private dialogRef: MatDialogRef<ItemCreateComponent>,
    private itemService: ItemService,
    privte dialogSerice: DialogService
  ) { }

  onFieldChange(field: keyof Item, e: any) {
    if(e.target.value != null && e.target.value != '') {
      (this.item as any)[field] = e.target.value;
    }
  }

  detectError() : boolean {
    let condition_text: boolean = this.item.itemCode != '' & this.item.enName != '' && this.item.vnName != '' && this.item.maker != '' && this.item.supplier != '' && this.item.positionIn != '' && this.item.unit != '' && this.item.currency != '';
    let condition_value = this.item.quantity > 0 && this.item.cost > 0;

    if(!(condition_text && condition_value)) return false;

    this.isShow = false;

    return true;
  }

  createItem() : void {
    if(!this.detectError()) {
      this.isShow = true;
      this.message = 'Data required!'

      if(this.item.quantity == 0 && this.item.cost == 0) this.message = 'Data required: Quantity, Cost > 0.';
      if(this.item.quantity == 0) this.message = 'Data required: Quantity > 0.';
      if(this.item.cost == 0) this.message = 'Data required: Cost > 0.';

      return;
    }

    this.itemService.createItem(this.item).subscribe({
      next: () => {
        this.dialogRef.close();

        this.dialogService.AutoAlterDialog({
          message: 'Create Item Success!',
          showcheck: true
        });
      },
      error: (err) => {
        this.dialogService.AutoAlterDialog({
          message: 'Create Item Failed!',
          showcheck: false
        });
      }
    });
  }
}
