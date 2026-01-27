import { NgModule } from "@angular/core";
import { ItemComponent } from "./item.component";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";
import { SharedModule } from "../../shared/shared.module";

@NgModule({
    declarations: [ItemComponent],
    imports: [
        CommonModule,
        SharedModule,
        RouterModule.forChild([
            { path: '', component: ItemComponent }
        ])
    ]
})
export class ItemModule { }
