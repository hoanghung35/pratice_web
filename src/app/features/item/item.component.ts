import { Component } from '@angular/core';
import { ItemService } from '../../core/services/item.service';

@Component({
  selector: 'app-item',
  standalone: false,
  templateUrl: './item.component.html',
  styleUrl: './item.component.scss'
})
export class ItemComponent {
  constructor(private itemService: ItemService) { }
  items: any
  res: any

  ngOnInit() {
    this.itemService.getItems().subscribe((res: any) => {
      this.items = res;
    });
  }
}
