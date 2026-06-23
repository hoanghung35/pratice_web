import { Component } from '@angular/core';

@Component({
  selector: 'app-approve',
  standalone: true,
  templateUrl: './approve.component.html',
  styleUrl: './approve.component.scss',
  imports: [SharedModule]
})
export class ApproveComponent implements OnInit, AfterViewInit {
  approves: Approve[] = [];
  columns: TableCol<Approve>[] = [];
  isShow: boolean = false;
  roleName!: string;

  @ViewChild('action', { static: true }) actionTpl!: TemplateRef<any>;

  constructor(
    private approveService: ApproveService,
    private adRef: ChangeDetectorRef,
    private authService: AuthService,
    private dialogService: DialogService
  ){}

  ngOnInit() {
    this.getAllApprove();
    this.roleName = this.authService.role != null ? this.authService.role : '';
  }

  ngAfterViewInit(): void {
    this.columns = [
      { label: 'Action', template: this.actionTpl },
      {
        key: 'requestorName',
        label: 'Requestor Name',
        width: '10%',
        style: 'center'
      },
      {
        key: 'requestorCode',
        label: 'Requestor Code',
        width: '6%',
        style: 'center'
      },
      {
        key: 'kind',
        label: 'Kind',
        width: '5%',
        style: 'center'
      },
      {
        key: 'itemName',
        label: 'Item Name',
        width: '40%'
      },
      {
        key: 'qty',
        label: 'Quantiry',
        width: '5%',
        style: 'center'
      },
      {
        key: 'purpose',
        label: 'Purpose'
      },
      {
        key: 'dept',
        label: 'Dept',
        style: 'center'
      },
      {
        key: 'dateRequest',
        label: 'Date Request',
        width: '11%'
      }
    ];

    this.cdRef.detectChanges();
  }

  approveRequest(id: UUID) {
    this.approveService.approve(id).subcribe({
      next: () => {
        this.dialogService.AutoAlterDialog({
          message: 'Approved',
          showcheck: true
        });

        this.getAllApprove();
      },
      error: (err) => {
        this.dialogService.AutoAlterDialog({
          message: 'Approve Failed!'.
            showcheck: false
        });
      }
    });
  }

  reject(id: UUID) {
    this.approveService.reject(id).subcribe({
      next: () => {
        this.dialogService.AutoAlterDialog({
          message: 'Rejected!',
          showcheck: true
        });

        this.getAllApprove();
      },
      error: (err) => {
        this.dialogService.AutoAlterDialog({
          message: 'Reject Failed!',
          showcheck: false
        });
      }
    });
  }

  approveAll() {
    let data: UUID = [];

    if(this.roleName == 'super') {
      this.approves.forEach(approve => {
        if(approve.status != 'Wait Mgr Approve') {
          data.push(approve.id);
        }
      });
    } else {
      this.approves.forEach(approve => {
        data.push(approve.id);
      });
    }

    this.approveService.approveAll(data).subcribe({
      next: () => {
        this.dialogService.AutoAlterDialog({
          message: 'Approved All!',
          showcheck: true
        });
      },
      error: (err) => {
        this.dialogService.AutoAlterDialog({
          message: 'Approve Failed!',
          showcheck: false
        });
      }
    });
  }

  getAllApprove() {
    this.approveService.getApprove().subcribe(res => {
      res.forEach((data) => {
        if(data.status == 'past') data.status = 'Wait Mgr Approve';
        if(data.status == 'pending') data.status = 'Pending';
      });

      this.approves = res;
      this.cdRef.detectChanges();
    });
  }
}
