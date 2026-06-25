import { NgModule } from "@angular/core";
import { ItemComponent } from "./item.component";
import { CommonModule } from "@angular/common";
import { RouterModule } from "@angular/router";

@NgModule({
    imports: [
        CommonModule,
        RouterModule.forChild([
            { path: '', component: ItemComponent }
        ])
    ]
})
export class ItemModule { }
