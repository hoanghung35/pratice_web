import { NgModule } from '@angular/core';
import { BrowserModule, provideClientHydration, withEventReplay } from '@angular/platform-browser';
import { MatIconModule } from '@angular/material/icon'


@NgModule({

    imports: [
        BrowserModule,
        MatIconModule,
    ]
})
export class ApproveModule { }