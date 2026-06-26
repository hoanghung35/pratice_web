





@Component({
  selector: 'app-upload-file-item',
  templateUrl: './upload-file-item.component.html',
  styleUrl: './upload-file-item.component.scss',
  standalone: true,
  imports: [SharedModule]
})
export class UploadFileItemComponent {
  file!: File;

  constructor(
    private itemService: ItemService,
    private dialogRef: MatDialogRef<UploadFileItemComponent>,
    private dialogService: DialogService
  ){}

  onFileChange(event: any) {
    this.file = event.target.files[0];
  }

  submit() {
    if(this.file) {
      this.itemService.createLlistItem(this.file).pipe(
        filter(event => event.type === HttpEventType.Response)
      ).subscribe({
        next: () => {
          this.dialogRef.close();

          this.dialogService.AutoAlterDialog({
            message: 'Create Item Success!',
            showcheck: true
          });
        },
        error: (err) => {
          this.dialogService.AutoAlterDialog({
            message: 'Create Item Failed!',
            showcheck: false
          });
        }
      });
    }
  }
}
