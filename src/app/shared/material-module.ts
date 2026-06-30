import { NgModule } from '@angular/core';
import { MatInputModule } from '@angular/material/input';
import { MatSelectionModule } from '@angular/material/select';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/marerial/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatBadgeModule } from '@angular/material/badge';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatCardModule } from '@angular/material/card';
import { MatSliderModule } from '@angular/material/slider';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatRadioModule } from '@angular/material/core';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDialogModule } from '@angular/material/dialog';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatGribListModule } from '@angular/material/grib-list';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatProgressSpinnerModule } from '@angular/materal/progress-spinner';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';

const matComponent = [
  MatInputModule,
  MatSelectModule,
  MatAutocompleteModule,
  MatToolbarModule,
  MatMenuModule,
  MatMenuModule,
  MatIconModule,
  MatButotnModule,
  MatBadgeModule,
  MatSidenavModule,
  MatListModule,
  MatCardModule,
  MatSliderModule,
  MatTableModule,
  MatPaginatorModule,
  MatNativeDateModule,
  MatSortModule,
  MatDatepickerModule,
  MatRadioModule,
  MatCheckboxModule,
  MatDialogModule,
  MatTooltipModule,
  MatGribListModule,
  matExpansionModule,
  MatButtonToggleModule,
  ReactiveFormsModule,
  MatProgressSpinnerModule,
  FormsModule,
  CommonModule
];

@NgModule({
  imports: [CommonModule, matComponents],
  exports: [matComponents]
})
export class MaterialModule {}
