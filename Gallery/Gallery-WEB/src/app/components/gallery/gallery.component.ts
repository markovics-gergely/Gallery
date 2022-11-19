import { ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AlbumDetailViewModel, PagerModel } from 'models';
import { PreviewService } from 'src/app/services/preview.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-gallery',
  templateUrl: './gallery.component.html',
  styleUrls: ['./gallery.component.scss']
})
export class GalleryComponent implements OnInit {
  private _pager: PagerModel | undefined;
  @Input() public gallery: AlbumDetailViewModel | undefined;

  constructor(
    private route: ActivatedRoute,
    private previewService: PreviewService,
    private userService: UserService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      console.log(params['id']);
    });
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
   * @param url Url of the image selected
   */
  delete(e: Event, url: string) {
    e.stopImmediatePropagation();
  }

  /** Count of images in the gallery */
  get itemCount() { return this.gallery?.pictureUrls?.length || 0; }
  /** Flag for gallery ownership */
  get ownGallery(): boolean { return this.gallery?.creatorId === this.userService.getActualUserId(); }
  /** Get sublist of the images by pager data */
  get pictures() {
    if (this._pager) {
      const length = this.gallery?.pictureUrls?.length || 0;
      const start = this._pager.page * this._pager.pageSize;
      let end = start + this._pager.pageSize;
      end = end > length ? length : end;
      return this.gallery?.pictureUrls?.slice(start, end);
    }
    return this.gallery?.pictureUrls;
  }
}