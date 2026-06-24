


@Component({
  selector: 'app-currency-create',
  templateUrl: './currency-create.component.html',
  styleUrl: './currency-create.component.scss',
  standalone: true,
  imports: [SharedModule]
})
export class CurrencyCreateComponent {
  data: Currency = {
    id: defaultKey,
    currentName: '',
    exchangeRate: 0
  };

  currency!: Currency;

  constructor(
    private currencyService: CurrencyService,
    private dialogRef: MatDialogRef<CurrencyCreateComponent>,
    private dialogService: DialogService
  ){}

  onChangeRate(field: any, res: any) {
    if(res == 0 || res == '') return;

    (this.data as any)[field] = res;
  }

  createCurrency() {
    if(this.data.currentName == '' || this.data.exchangeRate == 0) {
      this.dialogService.AutoAlterDialog({
        message: 'Data Require!',
        showcheck: false
      });
      return;
    }

    this.currencyService.createNewCurrency(this.data).subcribe({
      next: () => {
        this.dialogRef.close();

        this.dialogService.AutoAlterDialog({
          message: 'Create New Currency Success!',
          showcheck: true
        });
      },
      error: (err) => {
        this.dialogService.AutoAlterDialog({
          message: 'Create Failed!',
          showcheck: false
        });
      }
    });
  }
}
