import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { UsersComponent } from './users.component';
import { SharedModule } from '../../shared/shared.module';

@NgModule({
    imports: [
        CommonModule,
        UsersComponent,
        RouterModule.forChild([
            { path: '', component: UsersComponent }
        ])
    ]
})
export class UsersModule { }
