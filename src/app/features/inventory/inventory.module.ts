import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { InventoryComponent } from './inventory.component';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild([
      { path : '', component: InventoryComponent }
    ])
  ]
})
export class InventoryModule { }
