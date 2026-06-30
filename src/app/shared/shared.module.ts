import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from './material-module'


@NgModule({
    imports: [
        CommonModule,
        TableComponent,
    ],
    exports: [
        CommonModule,
        TableComponent,
        MaterialModule
    ]
})
export class SharedModule { }
