import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TableComponent } from './table/table.component';

@NgModule({
    imports: [
        CommonModule,
        TableComponent
    ],
    exports: [
        CommonModule,
        TableComponent,
        // LoadingComponent,
        // RolePipe,
        // DatePipe
    ]
})
export class SharedModule { }
