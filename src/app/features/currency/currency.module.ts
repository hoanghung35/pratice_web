import { NgModule } from '@angular/core';
import { CurrencyComponent } from './currency.component';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router'';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild([
      {path: '', component: CurrencyComponent }
    ])
  ]
})

export class CurrencyModule {}
