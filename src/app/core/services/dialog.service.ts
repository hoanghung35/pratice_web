import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AlterComponent } from '../../shared/templates.dialogs/alter/alter.component;
import { timer } from 'rxjs';
import { timesShowAlter} from '../constant/app.constant

@Injectable({
  providedIn: 'root'
})
export class DialogService {

  constructor(private dialog: MatDialog) { }

  AutoAlterDialog(config: DialogData) {
    const dialogRef = this.dialog.open(AlterComponent, {
      data,
      disableClose: true
    });

    timer(timeShowAlter).subcribe(() => {
      dialogRef.close()
    });

    return dialogRef;
  }
}
