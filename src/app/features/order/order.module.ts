import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { OrderComponent } from './order.component';
import { SharedModule } from '../../shared/shared.module';
import { RouterModule } from '@angular/router';

@NgModule({
    declarations: [OrderComponent],
    imports: [
        CommonModule,
        SharedModule,
        MatIconModule,
        RouterModule.forChild([{ path: '', component: OrderComponent }])
    ]
})
export class OrderModule { }