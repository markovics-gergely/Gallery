import { ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { AlbumViewModel, PagerModel } from 'models';
import { ConfirmAlbumService } from 'src/app/services/confirm-album.service';
import { PreviewService } from 'src/app/services/preview.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-gallery',
  templateUrl: './gallery.component.html',
  styleUrls: ['./gallery.component.scss']
})
export class GalleryComponent implements OnInit {
  private _pager: PagerModel | undefined;
  @Input() public gallery: AlbumViewModel | undefined;
  @Input() public needsCreator: boolean = false;

  constructor(
    private previewService: PreviewService,
    private userService: UserService,
    private confirmAlbumService: ConfirmAlbumService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.cdr.detectChanges();
  }

  /**
   * Process pager change events
   * @param value Pager changed event
   */
  setPage(value: PagerModel) {
    this._pager = value;
  }

  /**
   * Open preview page with the selected image
   * @param preview Url of selected image
   */
  preview(preview: string) {
    this.previewService.previewImage = preview;
  }

  /**
   * Delete the image selected from the gallery
   * @param e Selection event
   * @param id Identity of the image selected
   */
  delete(e: Event, id: string) {
    e.stopImmediatePropagation();
    this.confirmAlbumService.deletePictures(this.gallery?.id || '', { PictureIds: [id] })
      .add(() => this.gallery!.pictures = this.gallery?.pictures?.filter(p => p.id !== id));
  }

  /** Count of images in the gallery */
  get itemCount() { return this.gallery?.pictures?.length || 0; }
  /** Flag for gallery ownership */
  get ownGallery(): boolean { return this.gallery?.creator?.id === this.userService.getActualUserId(); }
  /** Get sublist of the images by pager data */
  get pictures() {
    if (this._pager) {
      const length = this.gallery?.pictures?.length || 0;
      const start = this._pager.page * this._pager.pageSize;
      let end = start + this._pager.pageSize;
      end = end > length ? length : end;
      return this.gallery?.pictures?.slice(start, end);
    }
    return this.gallery?.pictures;
  }
}
