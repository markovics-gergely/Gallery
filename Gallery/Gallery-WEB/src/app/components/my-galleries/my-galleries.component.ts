import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import {
  AddAlbumPicturesDTO,
  AlbumViewModel,
  CreateAlbumDTO,
  PagerModel,
} from 'models';
import { AlbumService } from 'src/app/services/album.service';
import { ConfirmAlbumService } from 'src/app/services/confirm-album.service';
import { LoadingService } from 'src/app/services/loading.service';
import { SnackService } from 'src/app/services/snack.service';
import { environment } from 'src/environments/environment';
import { AlbumDialogComponent } from '../dialogs/album-dialog/album-dialog.component';

@Component({
  selector: 'app-my-galleries',
  templateUrl: './my-galleries.component.html',
  styleUrls: ['./my-galleries.component.scss'],
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
    private confirmAlbumService: ConfirmAlbumService,
    private snackService: SnackService
  ) {}

  ngOnInit(): void {
    this.loadingService.isLoading = true;
    this.route.params.subscribe((params) => {
      if (params['id']) {
        this.albumService.getAlbum(params['id']).subscribe((album) => {
          this._gallery = album;
        });
      } else {
        this.albumService
          .getAlbums(
            environment.default_page_size,
            environment.default_page,
            undefined
          )
          .subscribe((albums) => {
            this._galleries = albums.values;
            this._total = albums.total;
          });
      }
      this.loadingService.isLoading = false;
    });
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
    this.albumService
      .getAlbums(value.pageSize, value.page + 1, undefined)
      .subscribe((albums) => {
        this._galleries = albums.values;
        this._total = albums.total;
      });
  }

  /**
   * Open gallery creator page
   */
  addGallery() {
    this.albumService.loadConfig().subscribe((config) => {
      const dialogRef: MatDialogRef<AlbumDialogComponent, CreateAlbumDTO> =
        this.dialog.open(AlbumDialogComponent, {
          width: '60%',
          data: config,
        });
      dialogRef.afterClosed().subscribe((result) => {
        this.loadingService.isLoading = true;
        if (result) {
          this.albumService
            .createAlbum(result)
            .subscribe((resp) => {
              this.router.navigate(['mygalleries', resp]);
            })
            .add(() => (this.loadingService.isLoading = false));
        }
      });
    });
  }

  /**
   * Navigate back to my gallery list
   */
  backToList() {
    this.router.navigate(['mygalleries']);
  }

  /**
   * Switch favorite status of the gallery
   * @param g Gallery to edit
   * @param event Selection event
   */
  addOrRemoveFavorite(g: AlbumViewModel | undefined, event: Event) {
    event.stopImmediatePropagation();
    this.confirmAlbumService.addOrRemoveFavorite(g).add(() => {
      this.loadingService.isLoading = false;
      if (!g?.isFavorite) {
        this.snackService.openSnackBar('Removed from favorites!', 'OK');
      }
    });
  }

  /**
   * Switch public status of the gallery
   * @param g Gallery to edit
   * @param event Selection event
   */
  setPublicStatus(g: AlbumViewModel | undefined, event: Event) {
    event.stopImmediatePropagation();
    this.confirmAlbumService.setPublicStatus(g);
  }

  deleteGallery(g: AlbumViewModel | undefined, event: Event) {
    event.stopImmediatePropagation();
    this.confirmAlbumService.deleteGallery(g).add(() => {
      this._galleries = this._galleries?.filter(
        (gallery) => gallery.id !== g?.id
      );
      this.backToList();
    });
  }

  /**
   * Add pictures to the gallery
   * @param event File upload event
   */
  addPictures(event: Event) {
    this.loadingService.isLoading = true;
    this.albumService.loadConfig().subscribe((config) => {
      const files = (event.target as HTMLInputElement)?.files;
      const filtered = Array.from(files || []).filter((file) => {
        if (file.size > config.maxUploadSize * 1024 * 1024) {
          this.snackService.openSnackBar(
            `${file.name} has exceeded the ${config.maxUploadSize} mb file limit!`,
            'OK'
          );
          return false;
        }
        return true;
      });
      if (filtered.length > config.maxUploadCount) {
        this.snackService.openSnackBar(
          `You can only upload ${config.maxUploadCount} pictures at a time`,
          'OK'
        );
      }
      const dto: AddAlbumPicturesDTO = {
        pictures: filtered.slice(
          0,
          Math.min(filtered.length, config.maxUploadCount)
        ),
      };
      this.albumService
        .addPictures(this._gallery?.id || '', dto)
        .subscribe(() => {
          this.snackService.openSnackBar(
            `${dto.pictures.length} Pictures added`,
            'OK'
          );
        })
        .add(() => {
          this.loadingService.isLoading = false;
          this._total -= dto.pictures.length;
        });
    });
  }

  /**
   * Get style class for the cover
   * @param count Count of cover
   * @returns Class to style with
   */
  getImgClass(count: number) {
    if (count > 4) return 'sub-9';
    if (count > 1) return 'sub-4';
    return 'sub-1';
  }

  get galleries() {
    return this._galleries;
  }
  get gallery() {
    return this._gallery;
  }
  get total() {
    return this._total;
  }
  get placeholderImage() {
    return 'https://via.placeholder.com/100x100.png?text=Gallery';
  }
}
