import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { ItemService } from '../../../../core/services/item.service';
import { Item } from '../../../../shared/models/item.model';

@Component({
  selector: 'app-create.item',
  standalone: false,
  templateUrl: './create.item.component.html',
  styleUrl: './create.item.component.scss'
})
export class CreateItemComponent {
  formData: any = {
    itemCode: '',
    enName: '',
    vnName: '',
    quantity: 0,
    deptId: '',
    areaId: '',
    unit: '',
    cost: 0,
    currency: '',
    maker: '',
    supplier: '',
    image: '',
    positionIn: ''
  };

  constructor(
    private dialogRef: MatDialogRef<CreateItemComponent>,
    private itemService: ItemService
  ) { }

  onCancel() {
    this.dialogRef.close();
  }

  onSubmit() {
    this.itemService.createItem(this.formData).subscribe((createdItem: Item) => {
      this.dialogRef.close(createdItem);
    });
  }

  onFieldChange(field: keyof Item, value: any) {
    this.formData[field] = value;
  }
}
