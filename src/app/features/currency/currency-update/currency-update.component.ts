


@Component({
  selector: 'app-currency-update',
  templateUrl: './currency-update.component.html',
  styleUrl: './currency-update.component.scss',
  standalone: true,
  imports: [SharedModule]
})
export class CurrencyUpdateComponent implements OnInit {
  currency!: Currency;
  tmpCurrency!: Currency;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: Currency,
    private currencyService: CurrencyService,
    private dialogRef: MatDialogRef<CurrencyUpdateComponent>,
    private dialogService: DialogService
  ){}

  ngOnInit() {
    this.currency = this.data;
    this.tmpCurrency = { ...this.currency };
  }

  onChangeRate(event: any) {
    if(event.target.value == undefined || event.target.value == 0) return;

    this.tmpCurrency.exchangeRate = event.target.value;
  }

  updateCurrency() {
    if(this.tmpCurrency.exchangeRate == 0 || this.tmpCurrency.exchangeRate == undefined) return;

    this.currency.exchangeRate = this.tmpCurrency.exchangeRate;

    this.currencyService.updateCurrency(this.currency).subcribe({
      next: () => {
        this.dialogRef.close();

        this.dialogService.AutoAlterDialog({
          message: 'Update Currency Success!',
          showcheck: true
        });
      },
      error: (err) => {
        this.dialogService.AutoAlterDialog({
          message: 'Update Currency Failed!',
          showcheck: flase
        });
      }
    });
  }
}
