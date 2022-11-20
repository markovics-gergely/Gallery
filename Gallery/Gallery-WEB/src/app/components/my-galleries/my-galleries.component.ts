import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { AlbumViewModel, PagerModel } from 'models';
import { AlbumService } from 'src/app/services/album.service';
import { ConfirmAlbumService } from 'src/app/services/confirm-album.service';
import { LoadingService } from 'src/app/services/loading.service';
import { AlbumDialogComponent } from '../dialogs/album-dialog/album-dialog.component';

@Component({
  selector: 'app-my-galleries',
  templateUrl: './my-galleries.component.html',
  styleUrls: ['./my-galleries.component.scss']
})
export class MyGalleriesComponent implements OnInit {
  private _galleries: AlbumViewModel[] | undefined;
  private _gallery: AlbumViewModel | undefined;
  private _total: number = 0;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private loadingService: LoadingService,
    private dialog: MatDialog,
    private albumService: AlbumService,
    private confirmAlbumService: ConfirmAlbumService
  ) { }

  ngOnInit(): void {
    this.loadingService.isLoading = true;
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.albumService.getAlbum(params['id']).subscribe(album => {
          this._gallery = album;
        });
      } else {
        this.albumService.getAlbums(10, 1, undefined).subscribe(albums => {
          this._galleries = albums.values;
          this._total = albums.total;
        });
      }
      this.loadingService.isLoading = false;
    })
  }

  /**
   * Navigate to the details of the selected gallery
   * @param value Identity of the selected gallery
   */
  selectGallery(value: string) {
    this.router.navigate(['mygalleries', value]);
  }

  /**
   * Process pager change events
   * @param value Pager changed event
   */
  setPage(value: PagerModel) {
    this.albumService.getAlbums(value.pageSize, value.page + 1, undefined).subscribe(albums => {
      this._galleries = albums.values;
      this._total = albums.total;
    });
  }

  /**
   * Open gallery creator page
   */
  addGallery() {
    const dialogRef = this.dialog.open(AlbumDialogComponent, {
      width: '60%',
      data: {}
    });
    dialogRef.afterClosed().subscribe(result => {
      this.loadingService.isLoading = true;
      console.log(result);
      this.albumService.createAlbum(result).subscribe(resp => {
        this.router.navigate(['mygalleries', resp]);
      }).add(() => this.loadingService.isLoading = false);
    });
  }

  /**
   * Navigate back to my gallery list
   */
  backToList() {
    this.router.navigate(['mygalleries']);
  }

  addOrRemoveFavorite(g: AlbumViewModel | undefined, event: Event) {
    event.stopImmediatePropagation();
    this.confirmAlbumService.addOrRemoveFavorite(g);
  }

  setPublicStatus(g: AlbumViewModel | undefined, event: Event) {
    event.stopImmediatePropagation();
    this.confirmAlbumService.setPublicStatus(g);
  }

  deleteGallery(g: AlbumViewModel | undefined, event: Event) {
    event.stopImmediatePropagation();
    this.confirmAlbumService.deleteGallery(g).add(() => this.backToList());
  }

  getImgClass(count: number) {
    if (count >=4) return 'sub-9';
    if (count >= 1) return 'sub-4';
    return 'sub-1';
  }

  get galleries() { return this._galleries; }
  get gallery() { return this._gallery; }
  get total() { return this._total; }
  get placeholderImage() { return 'https://via.placeholder.com/100x100.png?text=Gallery'; }
}
