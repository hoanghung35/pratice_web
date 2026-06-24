

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
  styleUrl: './history.component.scss',
  standalone: true,
  imports: [SharedModule]
})
export class HistoryComponent implements OnInit, AfterViewInit {
  histories: History[] = [];
  dataBackup: History[] = [];
  searchText = "";
  columns: TableCol<History>[] = [];

  constructor(
    private inventService: InventService,
    private cdRef: ChangeDetectorRef,
    private dialogService: DialogService
  ){ }

  ngAfterViewInit(): void {
    this.columns = [
      {
        key: 'empcode',
        label: 'Emp Code',
        width: '5%'
      },
      {
        key: 'qty',
        label: 'Quantity',
        width: '5%',
        style: 'center'
      },
      {
        key: 'reason',
        label: 'Reason',
        width: '17%'
      },
      {
        key: 'kind',
        label: 'Kind',
        width: '5%',
        style: 'center'
      },
      {
        key: 'dateAction',
        label: 'Date',
        width: '11%'
      },
      {
        key: 'pic',
        label: 'PIC'
      }
    ];

    this.cdRef.detectChanges();
  }

  ngOnInit(): void {
    this.getHistory();
  }
}
