import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

// import { TableComponent } from './components/table/table.component';
// import { LoadingComponent } from './components/loading/loading.component';

// import { RolePipe } from './pipes/role.pipe';
// import { DatePipe } from './pipes/date.pipe';

@NgModule({
    declarations: [
        // TableComponent,
        // LoadingComponent,
        // RolePipe,
        // DatePipe
    ],
    imports: [
        CommonModule
    ],
    exports: [
        CommonModule,
        // TableComponent,
        // LoadingComponent,
        // RolePipe,
        // DatePipe
    ]
})
export class SharedModule { }
