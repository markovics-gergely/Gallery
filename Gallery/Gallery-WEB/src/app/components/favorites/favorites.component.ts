import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Router, ActivatedRoute } from '@angular/router';
import { AlbumViewModel, PagerModel } from 'models';
import { AlbumService } from 'src/app/services/album.service';
import { ConfirmAlbumService } from 'src/app/services/confirm-album.service';
import { LoadingService } from 'src/app/services/loading.service';
import { SnackService } from 'src/app/services/snack.service';
import { UserService } from 'src/app/services/user.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-favorites',
  templateUrl: './favorites.component.html',
  styleUrls: ['./favorites.component.scss'],
})
export class FavoritesComponent implements OnInit {
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
    private userService: UserService,
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
          .getFavoriteAlbums(
            environment.default_page_size,
            environment.default_page
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
    this.router.navigate(['favorites', value]);
  }

  /**
   * Process pager change events
   * @param value Pager changed event
   */
  setPage(value: PagerModel) {
    this.albumService
      .getFavoriteAlbums(value.pageSize, value.page + 1)
      .subscribe((albums) => {
        this._galleries = albums.values;
        this._total = albums.total;
      });
  }

  /**
   * Navigate back to browse
   */
  backToList() {
    this.router.navigate(['favorites']);
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
      if (g?.isFavorite) {
        this.snackService.openSnackBar('Removed from favorites!', 'OK');
        this._galleries = this._galleries?.filter(
          (gallery) => gallery.id !== g?.id
        );
        this._total--;
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

  /**
   * Delete gallery and its' pictures
   * @param g Gallery to edit
   * @param event Selection event
   */
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
   * Get style class for the cover
   * @param count Count of cover
   * @returns Class to style with
   */
  getImgClass(count: number) {
    if (count > 4) return 'sub-9';
    if (count > 1) return 'sub-4';
    return 'sub-1';
  }

  isOwnGallery(g: AlbumViewModel) {
    return g.creator?.id === this.userId;
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
  get userId() {
    return this.userService.actualUserId;
  }
}
