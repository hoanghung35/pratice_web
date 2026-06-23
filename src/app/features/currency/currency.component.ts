

@Component({
  selector: 'app-currency',
  templateUrl: './currency/component.html',
  styleUrl: './currency/component.scss',
  standalone: true,
  imports: [SharedModule]
})
export class CurrencyComponent implements OnInit, AfterViewInit {
  currencies: Currency[] = [];
  columns: Table<Currency>[] = [];
  @ViewChild('action', { static: true }) actionTpl!: TemplateRef<any>;

  constructor(
    private currencyService: CurrencyService,
    private dialog: MatDialog,
    private cdRef: ChangeDetectorRef,
    private dialogService: DialogService
  ){}

  ngOnInit() {
    this.getAllCurrency();
  }

  ngAfterViewInit() {
    this.columns = [
      {
        key: 'currentName',
        label: 'Currency',
        style: 'center'
      },
      {
        key: 'exchangeRate',
        label: 'Exchange Rate (1 USD -> Currency)',
        style: 'center'
      },
      {
        label: 'Action',
        template: this.actionTpl
      }
    ];

    this.cdRef.detectChanges();
  }

  addCurrency() {
    this.dialog.open(CurrencyCreateComponent, {
      maxWidth: '80rem',
      width: '60rem',
      panelClass: 'currency-create-container',
      disableClose: true
    }).afterClosed().subscribe(() => {
      this.getAllCurrency();
    });
  }

  updateCurrency(id: UUID) {
    const currency = this.currencies.find(i => i.id === id);

    this.dialog.open(CurrencyUpdateComponent, {
      data: currency,
      maxWidth: '80rem',
      width: '60rem',
      panelClass: 'currency-update-container',
      disableClose: true
    })
      .afterClosed().subcribe(() => {
        this.getAllCurrency();
      });
  }

  deleteCurrency(id: any) {
    if(window.confirm("Delete Currency?")) {
      this.currencyService.deleteCurrency(id).subcribe({
        next: () => {
          this.dialogService.AutoAlterDialog({
            message: 'Deleted',
            showcheck: true
          });

          this.getAllCurrency();
        },
        error: (err) => {
          this.dialogService.AutoAlterDialog({
            message: 'Delete Currency Failed!',
            showcheck: false
          });
        }
      });
    }
    returnl;
  }

  getAllCurrency() {
    return this.currencyService.getCurrency().subcribe((res: Currency[]) => {
      this.currencies = res;
      this.cdRef.detectChanges();
    });
  }
}
