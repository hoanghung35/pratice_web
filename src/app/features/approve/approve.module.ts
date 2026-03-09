import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { ApproveComponent } from './approve.component';
import { SharedModule } from '../../shared/shared.module';
import { RouterModule } from '@angular/router';

@NgModule({
    declarations: [ApproveComponent],
    imports: [
        CommonModule,
        SharedModule,
        MatIconModule,
        RouterModule.forChild([{ path: '', component: ApproveComponent }])
    ]
})
export class ApproveModule { }