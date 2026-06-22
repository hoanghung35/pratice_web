import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { AccountComponent } from './account.component';
import { SharedModule } from '../../shared/shared.module';

@NgModule({
    imports: [
        CommonModule,
        AccountComponent,
        RouterModule.forChild([
            { path: '', component: AccountComponent }
        ])
    ]
})
export class AccountModule { }
