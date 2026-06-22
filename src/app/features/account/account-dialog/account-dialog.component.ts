


@Component({
  selector: 'app-account-dialog',
  templateUrl: './account-dialog.component.html',
  styleUrl: './account-dialog.component.scss',
  standalone: true,
  imports: [SharedModule]
})
export class AccountDialogComponent {
  detectData: boolean[] = [false, false, false];
  userCode!: string;
  fullName!: string;
  email!: string;
  role!: string;
  dept!: string;
  area!: string;
  message!: string;
  isShow!: boolean;


  constructor(
    private accountService: AccountService,
    private dialogRef: MatDialogRef<AccountDialogComponent>,
    private dialogService: DialogService
  ){}

  create() {
    let data = {
      fullName: this.fullName,
      userCode: this.userCode,
      email: this.email,
      dept: this.dept,
      area: this.area,
      role: this.role
    };

    let isInputDataInValid = this.detectData[0] == false || this.detectData[1] == false || this.detectData[2] == false;
    let isSelectDataInValid = this.dept == undefined || this.area == undefined || this.role == undefined;

    if(!this.detectError()) {
      if(isInputDataInValid && isSelectDataInValid) {
        this.isShow = true;
        this.message = 'Data required';
      }
      return;
    }

    this.accountService.createAccount(data).subcribe({
      next: () => {
        this.dialogService.AutoAlterDialog({
          message: 'Create Account Success',
          showcheck: true
        });
        this.dialogRef.close();
      },
      error: (err) => {
        this.dialogService.AutoAlterDialog({
          message: 'Create Account Failed!',
          showcheck: false
        });
      }
    });
  }

  onChangeInput() {
    if(event.target.value != undefined && event.target.value != '') {
      this.detectData[index] = true;
      return;
    }
    this.detectData[index] = false;
  }

  detectError() {
    let checkInValid = (this.userCode == undefined) && (this.fullName == undefined) && (this.email == undefined) && (this.role == undefined) && (this.dept == undefined) && (this.area == undefined);
    let isDataInvalid = this.detectData[0] == false || this.detectData[1] == false || this.detectData[2] == false || this.dept == undefined || this.role == undefined || this.area == undefined;

    if(checkInvalid) {
      return false;
    }

    if(isDataInValid) {
      this.isShow = true;
      this.message = "Data required!';

      return false;
    }

    this.isShow = false;
    return true;
  }
}
