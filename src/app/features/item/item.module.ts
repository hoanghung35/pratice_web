import { NgModule } from "@angular/core";
import { ItemComponent } from "./item.component";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import { SharedModule } from "../../shared/shared.module";
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { CreateItemComponent } from './item.dialog/create.item/create.item.component';

@NgModule({
    declarations: [ItemComponent, CreateItemComponent],
    imports: [
        CommonModule,
        SharedModule,
        MatDialogModule,
        MatFormFieldModule,
        MatInputModule,
        RouterModule.forChild([
            { path: '', component: ItemComponent }
        ])
    ]
})
export class ItemModule { }
