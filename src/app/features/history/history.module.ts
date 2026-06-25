import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HistoryComponent } from './history.component';
import { NgModule } from '@angular/core';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild([
      { paht: '', component: HistoryComponent }
    ])
  ]
})

export class HistoryModule { }
