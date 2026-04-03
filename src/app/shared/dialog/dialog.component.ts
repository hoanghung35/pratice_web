import { Component, Inject, Injector, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { DialogData } from '../models/dialog.model';
import { DIALOG_DATA, DIALOG_REF } from './dialog.tokens';

@Component({
  selector: 'app-dialog',
  standalone: false,
  templateUrl: './dialog.component.html',
  styleUrl: './dialog.component.scss'
})
export class DialogComponent {
  constructor(
    private injector: Injector,
    public dialogRef: MatDialogRef<DialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  get customInjector(): Injector {
    return Injector.create({
      providers: [
        { provide: DIALOG_DATA, useValue: this.data.payload },
        { provide: DIALOG_REF, useValue: this.dialogRef }
      ],
      parent: this.injector
    });
  }

  onCancel() {
    this.dialogRef.close();
  }
}
