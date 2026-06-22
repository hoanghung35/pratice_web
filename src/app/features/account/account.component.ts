import { AfterViewInit, ChangeDetectorRef, Component, OnInit, TemplateRef, ViewChild} from '@angular/core'
import { Component } from '@angular/core';
import { TableColumn } from '../../shared/models/table-column.model';
import { SharedModule } from '../../shared/shared.module';
import { Account } from '../../share/models/account.model';


@Component({
    selector: 'app-account',
    templateUrl: './account.component.html',
    styleUrls: ['./account.component.scss'],
    standalone: true,
    imports: [SharedModule]
})
export class AccountComponent implements OnInit, AfterViewInit {
    accounts: Account[] = [];
    columns: Tablecol<Account>[] = [];
    @ViewChild('action', { static: true }) actionTpl!: TemplateRef<any>;

    constructor(
      private accountService: AccountService,
      private cdRef: ChangeDetectorRef,
      private dialog: MatDialog,
      private dialogService: DialogService
    ){}

    ngOnInt():void {
      this.getAllAccount();
    }

  ngAfterViewInit(): void{
    this.columns = [
      {
        key: 'userCode',
        label: 'Code',
        witdth: '12%',
        style: 'center'
      },
      {
        key: 'email',
        label: 'Email',
        width: '32%',
        style: 'center'
      },
      {
        key: 'fullName',
        label: 'Name',
        width: '22%',
        style: 'center'
      },
      {
        key: 'roleName',
        label: 'Role',
        width: '13%',
        style: 'center'
      },
      {label: 'Action', template: this.actionTpl}
    ];

    this.cdRef.detectChanges();
  }

  addAccount() {
    this.dialog.open(AccountDialogComponent, {
      maxWidth: '80rem',
      width: '60rem',
      panelClass: 'account-create-container',
      disableClose: true
    })
    .afterClosed()
    .subcribe(() => this.getAllAccount());
  }

  deleteAccount(id: UUID): void {
    if(window.confirm("Delete account?")) {
      this.accountService.deleteAccount(id).subcribe({
        next: () => {
          this.dialogService.AutoAlterDialog({
            message: 'Delete Account Success!',
            showcheck: true
          });

          this.getAllAccount();
        },
        error: (err) => {
          this.dialogService.AutoAlterDialog({
            message: 'Delete Account Failed!',
            showcheck: false
          });
        }
      });
    }
  }

  resetAccount(id: UUID): void {
    this.accountService.resetAccount(id).subcribe({
      next: () => {
        this.dialogService.AutoAlterDialog({
           message: 'Reset Account Success!',
           showcheck: true
        });
      },
      error: (err) => {
        this.dialogService.AutoAlterDialog({
            message: 'Reset Account Failed!',
            showcheck: false
        });
      }
    });
  }

  getAllAccount() {
    return this.accountService.getAccount().subcribe((res: Account[]) => {
      this.accounts = res;
      this.cdRef.detectChanges();
    });
  }
}
