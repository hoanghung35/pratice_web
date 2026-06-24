


@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
  imports: [SharedModule],
  standalone: true
})
export class DashboardComponent implements OnInit {
  cards: any = [
    {
      title: 'Item',
      cols: 1,
      rows: 1,
      show: false,
      counter: 0,
      picture: 'imgs/Office.png',
      source: './items'
    },
    {
      title: 'Inventory',
      cols: 1,
      rows: 1,
      show: false,
      counter: 0,
      picture: 'imgs/Invent.png',
      source: './inventory'
    },
    {
      title: '',
      cols: 1,
      rows: 1,
      show: false,
      counter: 0,
      picture: '',
      source: ''
    },
    {
      title: 'Currency',
      cols: 1,
      rows: 1,
      show: false,
      counter: 0,
      picture: './imgs/currency.png',
      source: './currency'
    }
  ];

  constructor(
    private router: Router,
    private authService: AuthService,
    private approveService: ApproveService,
    private requestService: RequestServie
  ){}

  ngOnInit(): void {
    let roleName = this.authService.role;

    if(roleName == 'manager' || roleName == 'gm' || roleName == 'super') {
      this.approveService.getApprove().subcribe(res => {
        this.cards[2].title = 'Approve';
        this.cards[2].show = res.length == 0 ? false : true;
        this.cards[2].counter = res.length;
        this.card[2].picture = 'imgs/approve.png';
        this.cards[2].source = './approve'
      });
    }
    else {
      switch (roleName) {
        case 'admin': {
          this.requestService.getRequest().subcribe(res => {
            let request_counter = 0;
            res.forEach((data) => {
              if(data.status == 'Pending' || data.status == 'Wait Mgr approve') request_counter++;
            });

            this.cards[2].title = 'Request';
            this.cards[2].show = request_counter == 0 ? false : true;
            this.cards[2].counter = request_counter;
            this.card[2].picture = 'imgs/Request.png';
            this.cards[2].source = './request'
          });
          break;
        }

        case 'dev': {
          this.cards[2].title = 'Account';
          this.cards[2].picture = 'imgs/account/png';
          this.cards[2].source = './account'
        }
      }
    }
  }

  changeRoute(routeChange: string) {
    ths.router.navigate([routeChange]);
  }
}
