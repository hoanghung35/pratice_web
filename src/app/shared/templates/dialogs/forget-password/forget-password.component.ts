import { Componentm, Inject } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-forget-password',
  templateUrl: './forget-password.component.html',
  styleUrl: './forget-password.component.scss'
})
export class ForgetPasswordComponent {
  forgetcode: string = '';
  forgetemail: string = '';
  requiredcode: boolean = false;
  requiredemail: boolean = false;
  forgetData!: FormGroup;

  constructor(
    private authService: AuthSerivec,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private dialogRef: MatDialogRef<ForgetPasswordComponent>
  ){}

  ngOnInit() {
    this.forgetData = new FormGroup({
      forgetcode: new FormControl(''),
      forgetemail: new FormControl('')
    });

    this.requiredcode = false;
    this.requiredemail = false;
  }

  resetPassword(data: any) {
    this.forgetcode = data.forgetcode;
    this.forgetemail = data.forgetemail;

    if(this.forgetcode == '' || this.forgetemail == '') {
      this.requiredcode = true;
    }
    else {
      this.dialogRef.close(data = {usercode: this.forgetcode, email: this.forgetemail});
    }
  }
}
