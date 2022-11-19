import { Component, Input, OnInit } from '@angular/core';
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
  private _galleryId: string | undefined;
  private _pager: PagerModel | undefined;
  @Input() public gallery: AlbumDetailViewModel | undefined;

  constructor(
    private route: ActivatedRoute,
    private previewService: PreviewService,
    private userService: UserService
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this._galleryId = params['id'];
    })
  }

  setPage(value: PagerModel) {
    this._pager = value;
  }

  preview(preview: string) {
    this.previewService.previewImage = preview;
  }

  delete(e: Event, url: string) {
    e.stopImmediatePropagation();
  }

  get itemCount() {
    return this.gallery?.pictureUrls?.length || 0;
  }
  get ownGallery(): boolean { return this.gallery?.creatorId === this.userService.getActualUserId();}
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
