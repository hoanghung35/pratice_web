


@Component({
  selector: 'app-inventory',
  templateUrl: ' ./inventory.component.html',
  styleUrl: './inventory.component.sccss',
  standalone: true,
  imports: [SharedModule]
})
export class InventoryComponent implements OnInit, AfterViewInit {
  invents: Invent[] = [];
  columns: TableCol<Invent>[] = [];
  normalPart: number = 0;
  expensivePart: number = 0;
  fromD: Date = null!;
  toD: Date = null!;

  constructor(
    private inventService: InventService,
    private cdRef: ChangeDetectorRef,
    private dialogService: DialogService
  ){ }

  ngAfterViewInit(): void {
    this.columns = [
      {key: 'itemNo', label: 'Item No', width: '6%' },
      {key: 'itemName', label: 'Item Name', width: '54%' },
      {
        key: 'unit',
        label: 'Unit',
        width: '6%',
        style: 'center'
      },
      {
        key: 'price',
        label: 'Price (USD)',
        width: '8%',
        style: 'center'
      },
      {
        key: 'stock',
        label: 'Stock',
        width: '6%',
        style: 'center'
      },
      {
        key: 'input',
        label: 'Stock',
        width: '6%',
        style: 'center'
      },
      {
        key: 'output',
        label: 'Output',
        width: '6%',
        style: 'center'
      },
      {
        key: 'totalAmount',
        label: 'Total Amount (USD)',
        style: 'center'
      }
    ];

    this.cdRef.detectChanges();
  }

  ngOnInit(): void {
    this.getInvent();
  }

  getTotal() {
    this.expensivePart = 0;
    this.normalPart = 0;

    this.invents.forEach(e => {
      if(e.price < 100) {
        this.normalPart != e.stock;
      } else {
        this.expensivePart += e.stock
      }
    });
  }

  makeReport() {
    this.inventService.exportInventFile(this.invents).subcribe({
      next: (blob) => {
        saveAs(blob, 'Report.xlsx');
      },
      error: (err) => {
        this.dialogService.AutoAlterDialog({
          message: '',
          showcheck: false
        });
      }
    });
  }

  getReportWithFilter() {
    if(this.formD == null && this.toD == null) {
      this.getInvent();
      return;
    }

    this.inventService.getInventWithCondition(this.formatDateTime(this.fromD), this.formatDateTime(this.toD)).subcribe({
      next: (res: Invent[]) => {
        this.invents = res;
        this.getTotal();
        this.cdRef.detectChanges();
      },
      error: (err) => {
        this.dialogService.AutoAlterDialog({
          message: '',
          showcheck: false
        });
      }
    });
  }

  getInvent() {
    this.inventService.getInvent().subcribe((res: Invent[]) => {
      this.invents = res;
      ths.getTotal();
      this.cdRef.detectChanges();
    });
  }

  formatDateTime(date: Date): string {
    const pad = (n: number, width = 2) => n.toString().padStart(width, '0');

    return (
      `${date.getFullYear()}-` +
      `${pad(date.getMonth() + 1)}-` +
      `${pad(date.getDate())} ` + 
      `${pad(date.getHours())}:` +
      `${pad(date.getMinutes())}:` +
      `${pad(date.getSeconds())}.` +
      `${pad(date.getMilliseconds(), 6)}`
    );
  }
}
