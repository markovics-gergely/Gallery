import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AlbumViewModel, PagerModel } from 'models';
import { AlbumService } from 'src/app/services/album.service';
import { ConfirmAlbumService } from 'src/app/services/confirm-album.service';
import { LoadingService } from 'src/app/services/loading.service';
import { SnackService } from 'src/app/services/snack.service';
import { TokenService } from 'src/app/services/token.service';
import { UserService } from 'src/app/services/user.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-browse',
  templateUrl: './browse.component.html',
  styleUrls: ['./browse.component.scss'],
})
export class BrowseComponent implements OnInit {
  private _galleries: AlbumViewModel[] | undefined;
  private _gallery: AlbumViewModel | undefined;
  private _total: number = 0;
  private _canLike: boolean = true;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private loadingService: LoadingService,
    private albumService: AlbumService,
    private confirmAlbumService: ConfirmAlbumService,
    private userService: UserService,
    private snackService: SnackService,
    private tokenService: TokenService
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
          .getBrowsableAlbums(
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
    this.router.navigate(['browse', value]);
  }

  /**
   * Process pager change events
   * @param value Pager changed event
   */
  setPage(value: PagerModel) {
    this.albumService
      .getBrowsableAlbums(value.pageSize, value.page + 1)
      .subscribe((albums) => {
        this._galleries = albums.values;
        this._total = albums.total;
      });
  }

  /**
   * Navigate back to browse
   */
  backToList() {
    this.router.navigate(['browse']);
  }

  addOrRemoveFavorite(g: AlbumViewModel | undefined, event: Event) {
    event.stopImmediatePropagation();
    this.confirmAlbumService.addOrRemoveFavorite(g).add(() => {
      this.loadingService.isLoading = false;
      if (!g?.isFavorite) {
        this.snackService.openSnackBar('Removed from favorites!', 'OK');
      }
    });
  }

  likeGallery(g: AlbumViewModel | undefined, event: Event) {
    event.stopImmediatePropagation();
    this.confirmAlbumService.likeGallery(g).add(() => {
      this._canLike = false;
      setTimeout(() => (this._canLike = true), 5000);
      this.loadingService.isLoading = false;
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

  /**
   * Getter for user administrator status
   */
  get isAdmin(): boolean {
    return this.tokenService.role === 'Admin';
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
  get canLike() {
    return this._canLike;
  }
}
