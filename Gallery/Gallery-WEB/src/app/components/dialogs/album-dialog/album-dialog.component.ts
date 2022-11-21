import { Component, Inject, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CreateAlbumDTO } from 'models';

@Component({
  selector: 'app-album-dialog',
  templateUrl: './album-dialog.component.html',
  styleUrls: ['./album-dialog.component.scss']
})
export class AlbumDialogComponent implements OnInit {
  private _form: FormGroup | undefined;

  constructor(
    public dialogRef: MatDialogRef<AlbumDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: CreateAlbumDTO,
    private formBuilder: FormBuilder
  ) { }

  ngOnInit(): void {
    this._form = this.formBuilder.group({
      name: new FormControl('', Validators.required),
      isPrivate: new FormControl(false, Validators.required),
      pictures: this.formBuilder.array([])
    });
  }

  onSubmit() {
    const dto: CreateAlbumDTO = {
      name: this._form?.get('name')?.value,
      isPrivate: this._form?.get('isPrivate')?.value,
      pictures: this._form?.get('pictures')?.value.map((f: { file: File; }) => f.file)
    };
    console.log(dto);
    this.dialogRef.close(dto);
  }

  exit() {
    this.dialogRef.close();
  }

  addPicture(event: Event) {
    const files = (event.target as HTMLInputElement)?.files;
    if (files) {
      Array.from(files).forEach(file => {
        (this._form?.get('pictures') as FormArray).push(
          new FormGroup({
            file: new FormControl(file, Validators.required)
          })
        );
      });
    }
  }

  getFilename(index: number): string {
    return (this._form?.get('pictures') as FormArray).value[index]?.file?.name
  }
  removePicture(index: number) {
    (this._form?.get('pictures') as FormArray).removeAt(index);
  }

  get pictureControls() { return (this._form?.get('pictures') as FormArray).controls; }
  get form() { return this._form; }
}
