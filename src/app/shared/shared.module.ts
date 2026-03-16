import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { TableComponent } from './table/table.component';

@NgModule({
    imports: [
        CommonModule,
        MatButtonModule,
        MatIconModule,
        TableComponent
    ],
    exports: [
        CommonModule,
        MatButtonModule,
        MatIconModule,
        TableComponent,
        // LoadingComponent,
        // RolePipe,
        // DatePipe
    ]
})
export class SharedModule { }
