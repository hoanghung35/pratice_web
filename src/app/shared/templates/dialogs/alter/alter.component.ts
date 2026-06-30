import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

export interface DialogData {
  message: string;
  showcheck: boolean;
}

@Component({
  selector: 'app-alter',
  templateUrl: './alter.component.html',
  styleUrl: './alter.component.scss',
})
export class AlterComponent implements OnInit {
  success: boolean = false;
  failed: boolean = false;
  alterData!: DialogData;

  constructor(MAT_DIALOG_DATA public data: DialogData) {}

  ngOnInit() {
    this.alterData = this.data;

    if(this.alterData.showcheck) {
      this.success = true;
      this.failed = false;
    } else {
      this.failed = true;
      this.success = false;
    }
  }
}
