



@Component({
  selector: 'app-request',
  standalone: false,
  templateUrl: './request.component.html',
  styleUrl: './request.component.scss'
})
export class RequestComponent implements OnInit, AfterViewInit {
  requests: Request[] = [];

  columns: TableColumn<Request>[] = [];

  constructor(
    private requestService: RequestService,
    private cdRef: ChangeDetectorRef
  ) { }

  ngAfterViewInit(): void {
    this.columns = [
      { key: 'enName', label: 'Item Name' },
      { key: 'maker', label: 'Maker' },
      { key: 'quantity', label: 'Quantity' },
      { key: 'dateOrder', label: 'Date' }
    ];
    this.cdRef.detectChanges();
  }

  ngOnInit(): void {
    this.orderService.getOrder().subscribe((res: any) => {
      this.orders = res;
      this.cdRef.detectChanges();
    })
  }

}
