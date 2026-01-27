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

  ngOnInit() {
    this.itemService.getItems().subscribe(res => {
      this.items = res;
    });
  }
}
