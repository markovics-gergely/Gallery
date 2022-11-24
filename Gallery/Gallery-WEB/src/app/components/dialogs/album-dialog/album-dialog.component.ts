import { Component, Inject, OnInit } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ConfigViewModel, CreateAlbumDTO } from 'models';
import { LoadingService } from 'src/app/services/loading.service';
import { SnackService } from 'src/app/services/snack.service';

@Component({
  selector: 'app-album-dialog',
  templateUrl: './album-dialog.component.html',
  styleUrls: ['./album-dialog.component.scss'],
})
export class AlbumDialogComponent implements OnInit {
  private _form: FormGroup | undefined;

  constructor(
    public dialogRef: MatDialogRef<AlbumDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ConfigViewModel,
    private formBuilder: FormBuilder,
    private loadingService: LoadingService,
    private snackService: SnackService
  ) {}

  ngOnInit(): void {
    this._form = this.formBuilder.group({
      name: new FormControl('', Validators.required),
      isPrivate: new FormControl(false, Validators.required),
      pictures: this.formBuilder.array([]),
    });
  }

  onSubmit() {
    const dto: CreateAlbumDTO = {
      name: this._form?.get('name')?.value,
      isPrivate: this._form?.get('isPrivate')?.value,
      pictures: this._form
        ?.get('pictures')
        ?.value.map((f: { file: File }) => f.file),
    };
    this.dialogRef.close(dto);
  }

  exit() {
    this.dialogRef.close();
  }

  addPicture(event: Event) {
    const files = (event.target as HTMLInputElement)?.files;
    let overloaded = false;
    Array.from(files || []).forEach((file) => {
      if (file.size <= this.data.maxUploadSize * 1024 * 1024) {
        if (this.uploadedCount < this.data.maxUploadCount) {
          (this._form?.get('pictures') as FormArray).push(
            new FormGroup({
              file: new FormControl(file, Validators.required),
            })
          );
        } else {
          overloaded = true;
        }
      } else {
        this.snackService.openSnackBar(
          `${file.name} has exceeded the ${this.data.maxUploadSize} mb file limit!`,
          'OK'
        );
      }
    });
    if (overloaded) {
      this.snackService.openSnackBar(
        `You can only upload ${this.data.maxUploadCount} pictures at a time`,
        'OK'
      );
    }
  }

  getFilename(index: number): string {
    return (this._form?.get('pictures') as FormArray).value[index]?.file?.name;
  }
  removePicture(index: number) {
    (this._form?.get('pictures') as FormArray).removeAt(index);
  }

  get pictureControls() {
    return (this._form?.get('pictures') as FormArray).controls;
  }
  get form() {
    return this._form;
  }
  get uploadedCount() {
    return (this._form?.get('pictures') as FormArray).controls.length;
  }
  get uploadFull() {
    return this.uploadedCount === this.data.maxUploadCount;
  }
}
