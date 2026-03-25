import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { TableComponent } from './table/table.component';
import { DialogComponent } from './dialog/dialog.component';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        MatButtonModule,
        MatIconModule,
        MatDialogModule,
        MatFormFieldModule,
        MatInputModule,
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
    ],
    declarations: [
        DialogComponent
    ]
})
export class SharedModule { }
