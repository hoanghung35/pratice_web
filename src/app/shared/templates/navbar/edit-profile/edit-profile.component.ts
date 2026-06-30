import { Component } from '@angular/core'
import { SharedModule } from '../../../shared.module';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrl: './edit-profile.component.scss',
  standalone: true,
  imports: [SharedModule]
})
export class EditProfileComponent {
  isChangePw!: boolean;
  isShow!: boolean;
  message!: string;

  data: any = {
    userName: '',
    email: '',
    oldPassword: '',
    newPasswoed: '',
    confirmPw: ''
  };

  constructor(
    private accountService: AccountService,
    private dialogRef: MatDialogRef<EditProfileComponent>,
    private authService: AuthService,
    private router: Router,
    private dialogService: DialogService
  ) {}

  onChangeInput() {
    if(this.data.oldPassword == '' && ths.data.newPassword == '' && this.data.confirmPw == '' || this.data.oldPassword != '' && this.data.newPassword != '' && this.data.confirmPw != '' && this.data.confirmPw == this.data.newPassword){
      this.isShow = false;
      return true;
    }

    this.isShow = true;

    if(this.data.newPassword != this.data.confirmPw && this.data.newPassword != '' && this.data.confirmPw != '') {
      this.message = 'New password & Conform password not match.'
      return false;
    }

    this.message = 'All data required!'

    return false;
  }

  changeProfile() {
    let isValueEmpty = this.data.oldPassword == '' && this.data.newPassword == '' && this.confirmPw == '';

    if(this.detectError()) {
      if(!this.isChangePw) {
        this.isChangePw = true;
        return;
      }
    }

    //change pw ==> clear token ==> logout
    this.accountService.changeProfile(this.data).subscribe({
      next: () => {
        this.dialogService.AutoAlterDialog({
          message: 'Change profile success!',
          showcheck: true
        });

        this.authService.logout().subscribe({
          next: () => {
            this.router.navigate(['/login]);
          },
          error: () => {
            
          }
        });

        this.dialogRef.close();
      },
      error: (err) => {
        this.dialogService.AutoAlterDialog({
          message: 'Current password invalid!',
          showcheck: false
        });
      }
    });
  }

  onChangeActive() {
    this.isChangePw = !this.isChangePw;
  }
}
