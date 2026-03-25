import { Injectable } from '@angular/core';
import { DialogComponent } from '../../shared/dialog/dialog.component';
import { DialogData } from '../../shared/models/dialog.model';
import { MatDialog } from '@angular/material/dialog';

@Injectable({
  providedIn: 'root'
})
export class DialogService {

  constructor(private dialog: MatDialog) { }

  openCustomDialog(config: DialogData) {
    return this.dialog.open(DialogComponent, {
      width: '400px',
      data: config
    });
  }
}
