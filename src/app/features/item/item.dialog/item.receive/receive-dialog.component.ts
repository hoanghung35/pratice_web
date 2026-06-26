





@Component({
  selector: 'app-receive-dialog',
  templateUrl: './receive-dialog.component.html',
  styleUrl: './receive-dialog.component.scss',
  standalone: true,
  imports: [SharedModule]
})
export class ReceiveDialogComponent {
  item!: Item;
  receiveQty!: number;
  confirmQty!: number;
  message!: string;
  srcImage!: string;
  isChange: boolean[] = [false, false];
  isShow!: boolean;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: Item,
    private itemService: ItemService,
    private dialogRef: MatDialogRef<ReceiveDialogComponent>,
    private dialogService: DialogService
  ){}

  ngOnInit() {
    this.item = this.data;

    if(this.item.image != null && this.item.iamge != "") {
      this.srcImage = `/imgs/items/${this.item.image}`;
    } else {
      this.srcImage = '/imgs/items/default.png';
    }
  }

  receive(){
    if(!this.detectError()) return;

    this.itemService.receiveItem(this.item, this.confirmQty).subscribe({
      next: () => {
        this.item.quantity += Number(this.receiveQty);
        this.dialogRef.close(this.item);

        this.dialogService.AutoAlterDialog({
          message: '',
          showcheck: true
        });
      },
      error: () => {
        this.dialogService.AutoAlterDialog({
          message: '',
          showcheck: flase
        });
      }
    });
  }

  detectError() {
    if(this.confirmQty === this.receiveQty && this.receiveQty > 0 && this.isChange[0] && this.isChange[1]) {
      this.message = '';
      this.isShow = false;

      return true;
    }

    var condition = (this.isChange[0] == false && this.isChange[1] == true) || (this.isChange[0] == true && this.isChange[1] == false);

    if(this.receiveQty == undefined && this.confirmQty == undefined && this.isChange[0] == false && this.isChange[1] == false) {
      this.isShow = false;

      return false;
    }

    if(condition) this.message = 'Data is required!';

    if(this.receiveQty > 0 && this.confirmQty > 0 && this.confirmQty != this.receiveQty) this.message = 'Data does not match';

    if(this.receiveQty > 0 && this.confirmQty == undefined) this.message = 'Confirm Qty is required';

    this.isShow = true;
    return false;
  }

  onChangeInput(event: any, index: any) {
    if(event.target.value != '' && event.target.value != null) {
      this.isChange[index] = true;

      this.receiveQty = index == 0 ? event.target.value : this.receiveQty;
      this.confirmQty = index == 1 ? event.target.value : this.confirmQty;
      return;
    }
    this.isChange[index] = false;
  }
}
