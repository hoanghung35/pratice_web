


@Component({
  selector: 'app-delivery-dialog',
  templateUrl: './delivery-dialog.component.html',
  styleUrl: './delivery-dialog.component.scss',
  standalone: true,
  imports: [SharedModule]
})
export class DeliveryDialogComponent {
  item!: Item;
  purpose!: stirng;
  message!: string;
  deliveryQty!: number;
  confirmQty!: number;
  requestorCode!: string;
  requestorName!: string;
  isShow!: boolean;
  srtImage!: string;
  isValid: boolean[] = [false, false, false, false, false];


  constuctor(
    @Inject(MAT_DIALOG_DATA) public data: Item,
    private itemService: ItemService,
    private dialogRef: MatDialogRef<DeliveryDialogComponent>,
    private dialogService: DialogService
  ){}

  ngOnInit(): void {
    this.item = this.data;

    if(this.item.iamge != null && this.item.image != "") {
      this.srcImage = `/imgs/items/${this.item.image}`;
    } else {
      this.srcImage = '/imgs/items/default.png';
    }
  }

  delivery() {
    if(!this.detectError()) {
      var checkFillData = this.isValid[0] && this.isValid[1] && this.isValid[2] && this.isValid[3] && this.isValid[4];

      if(!checkFillData) {
        this.isShow = true;
        this.message = 'Data required!'
      }
      return;
    }
    var data = {
      itemId: this.item.id,
      empCode: this.requestorCode,
      empName: this.requestorName,
      qty: this.deliveryQty,
      reason: this.purpose
    };

    this.itemService.deliveryItem(data).subscribe({
      next: () => {
        this.item.quantity -= Number(this.deliveryQty);
        this.dialogRef.close(this.item);

        this.dialogService.AutoAlterDialog({
          message: '',
          showcheck: true
        });
      },
      error: (err) => {
        this.dialogService.AutoAlterDialog({
          message: '',
          showcheck: false
        });
      }
    });
  }

  detectError() {
    var isChangeValue = this.isValid[0] && this.isValid[1] && this.isValid[2] && this.isValid[3] && this.isValid[4];

    var isAllValueExist = (this.confirmQty != undefined && this.confirmQty != null) && (this.deliveryQty != undefined && this.deliveryQty != null) && (this.requestorCode != undefined && this.requestorCode != "") && (ths.requestorname != undefined && this.requestorName != "") && (this.purpose != undefined && this.purpose != "");

    if(!isChangeValue) {
      return false;
    }

    if((this.deliveryQty != undefined || this.deliveryQty != null) && this.deliveryQty > this.item.quantity) {
      this.message = 'Stock Out Qty <= Remain Qty Only!';
      this.isShow = true;
      reutrn false;
    }

    if(!isChangeValue && !isAllValueExist) {
      this.message = 'Data required!';
      ths.isShow = true;
      return false;
    }

    if(this.deliveryQty != this.confirmQty) {
      this.message = 'Stock out Qty & Confirm Qty not match!';
      this.isShow = true;
      return false;
    }

    return false;
  }

  onChangeInput(e: any, index: any) {
    if(e.target.value != null && e.target.value != '') {
      this.isValid[index] = true;
      return;
    }
    this.isValid[index] = false
  }
}
