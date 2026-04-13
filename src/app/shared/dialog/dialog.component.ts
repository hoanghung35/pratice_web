import { Component, Inject, Injector } from '@angular/core';
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
  formData: { [key: string]: any } = {};

  constructor(
    private injector: Injector,
    public dialogRef: MatDialogRef<DialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
  ) {
    this.initializeFormData();
  }

  private initializeFormData() {
    if (this.data?.type === 'form' && Array.isArray(this.data.fields)) {
      this.data.fields.forEach(field => {
        this.formData[field.name] = field.value ?? '';
      });
    }
  }

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

  onFieldInput(field: any, event: Event) {
    const input = event.target as HTMLInputElement | null;
    const value = input?.value ?? '';
    this.formData[field.name] = field.type === 'number' ? Number(value) : value;
  }

  onOk() {
    if (this.data?.type === 'form') {
      this.dialogRef.close(this.formData);
      return;
    }
    this.dialogRef.close(true);
  }
}
