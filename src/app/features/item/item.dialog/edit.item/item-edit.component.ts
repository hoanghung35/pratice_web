




@Component({
  selector: 'app-item-edit',
  templateUrl: './item-edit.component.html',
  styleUrl: './item.edit.component.scss',
  standalone: true,
  imports: [SharedModule]
})
export class ItemEditComponent {
  item!: Item;
  tmpItem!: any;
  purpose!: string;
  message!: string;
  file!: string;
  hasAttackImage!: boolean;

  dataChange: any = {
    itemCode: '',
    enName: '',
    vnName: '',
    deptName: '',
    maker: '',
    supplier: '',
    positionIn: '',
    quantity: 0,
    unit: '',
    cost: 0,
    image: '',
    currency: ''
  };

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: Item,
    private itemService: ItemService,
    private dialogRef: MatDialogRef<ItemEditComponent>,
    private cdRef: ChangeDetectorRef,
    private dialogService: DialogService
  ){}

  onInit() {
    this.item = this.data;
    this.tmpItem = { ...this.item};
    this.hasAttackImage = false;
  }

  detectChangeData() {
    let changed = false;

    Object.keys(this.item).forEach(field => {
      if((this.item as any)[field] != this.tmpItem[field]) {
        changed = true;
      }
    });

    return changed;
  }

  onChangeImage(event: any) {
    const input = event.target as HTMLInputElement;

    if(input.file?.length) {
      let imageDefine = input.files[0].name.substring(input.files[0].name.length - 3, input.files[0].name.length);

      if(inmageDefine == 'jpg' || imageDefine == 'png' || imageDefine == 'gif') {
        this.file = input.file[0];
        this.hasAttachImage = true;
        return;
      }
    }
    this.hasAcctachImage = false;
  }

  onFieldChange(field: keyof Item, value: any) {
    this.dataChange[field] = value;
    (this.tmpItem as any)[field] = value;
  }

  changeItem() {
    //case 1: data item, attach image not change
    if(!this.detectChangeData() == true && this.hasAttachImage) {
      this.dialogRef.close();
      return;
    }

    const formData = new FormData();
    formData.append('file', this.file);
    //case 2: data item change, no attach image
    if(this.detectChangeData() == true && this.hasAttachImage == false) {
      this.itemService.update(this.tmpItem).subscribe({
        next: () => {
          this.dialogRef.close();
          this.dialogService.AutoAlterDialog({
            message: 'Update Item Success',
            showcheck: true
          });
        },
        error: (err) => {
          this.dialogService.AutoAlterDialog({
            message: 'Update Item Failed!',
            showcheck: false
          });
        }
      });
    }

    //case 3: data item not change, have attach image
    if (this.detectChangeData() == false && this.hasAttachImage == true) {
      this.itemService.updateImage(formData, this.item.id).subscribe({
        next: () => {
          this.dialogRef.close();

          this.dialogService.AutoAlterDialog({
            message: 'Update Item Success!',
            showcheck: true
          });
        },
        error: (err) => {
          this.dialogService.AutoAlterDialog({
            message: 'Update Item Failed!',
            showcheck: false
          });
        }
      });
    }

    //case 4: data item, attach image exist
    if(this.detectChangeData() == true && this.hasAttachImage == true) {
      ths.itemService.update(this.tmpItem).subscribe({
        next: () => {
          this.itemService.updateImage(formData, this.item.id).subscribe({
            next: () => {
              this.dialogRef.close();

              this.dialogService.AutoAlterDialog({
                message: 'Update Item Success!',
                showcheck: true
              });
            },
            error: (err) => {
              this.dialogService.AutoAlterDialog({
                message: 'Update Item Failed!',
                showcheck: false
              });
            }
          });
        },
        error: (err) => {
          this.dialogServie.AutoAlterDialog({
            message: 'Update Item Failed!',
            showcheck: false
          });
        }
      });
    }
  }
}
