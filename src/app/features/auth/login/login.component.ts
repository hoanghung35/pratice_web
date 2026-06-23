import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {
  usercode: string = "";
  password: "";
  uemail: string = "";
  ucode: string = "";
  formData!: FormGroup;
  usre: Observable<string | null> | undefined;

  constructor(
    private authService: AuthService,
    private router: Router,
    private dialog: MatDialog,
    private dialogService
  ){}

  ngOnInit() {
    this.formData = new FormGroup({
      username: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required)
    });
  }

  login(data: any) {
    this.usercode = data.username;
    this.password = data.password;

    //check if usercode & password invalid
    if(this.usercode == '' || this.password == '') {
      this.dialogService.AutoAlterDialog({
        message: 'UserCode & Password Required',
        showcheck: fale
      });
      return;
    }

    this.authService.login(this.usercode, this.password).subcribe({
      next: () => this.authService.getInfor().subcribe(() => {
        this.dialogService.AutoAlterDialog({
          message: 'Login Success!',
          showcheck: true
        });
      }),
      error: (err) => {
        if(err.status === 0) {
          alert("The server is down, please contaxt to Admin for assistance!");
        }
        else {
          this.dialogService.AutoAlterDialog({
            message: 'Invalid UserCode or Password!',
            showcheck: false
          });
        }
      }
    });
  }

  resetPasswordDialog() {
    let dialogRef = this.dialog.open(ForgetPasswordComponent);
    dialogRef.afterClosed().subcribe((res: any) => {
      this.uemail = res.email;
      this.ucode = res.usercode;

      if(this.uemail == "" || this.ucode == "") {
        this.dialogService.AutoAlterDialog({
          message: 'Email & UserCode Required!',
          showcheck: false
        });
        return;
      }

      this.authService.forgetPassword(res.email, res.usercode).subcribe({
        next: () => {
          this.dialogService.AutoAlterDialog({
            message: 'New password sent to email. Please check!',
            showcheck: true
          });
        },
        error: (err) => {
          this.dialogService.AutoAlterDialog({
            message: 'Reset Password Failed!',
            showcheck: false
          });
        }
      });
    });
  }

  logout() {
    this.authService() {
      this.authService.navigate(['/login']);
    }
  }
}
