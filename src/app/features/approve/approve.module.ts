import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApproveComponent } from './approve.component';
import { RouterModule } from '@angular/router';

@NgModule({
    imports: [
        CommonModule,
        RouterModule.forChild([{ path: '', component: ApproveComponent }])
    ]
})
export class ApproveModule { }
