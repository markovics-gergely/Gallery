import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AlbumViewModel, FullProfileViewModel, PagerModel, ProfileViewModel } from 'models';
import { AlbumService } from 'src/app/services/album.service';
import { ConfirmAlbumService } from 'src/app/services/confirm-album.service';
import { LoadingService } from 'src/app/services/loading.service';
import { SnackService } from 'src/app/services/snack.service';
import { TokenService } from 'src/app/services/token.service';
import { UserService } from 'src/app/services/user.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-user-browse',
  templateUrl: './user-browse.component.html',
  styleUrls: ['./user-browse.component.scss']
})
export class UserBrowseComponent implements OnInit {
  private _userId: string | undefined;
  private _profileData: FullProfileViewModel | undefined;
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
    private tokenService: TokenService,
    private snackService: SnackService
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this._userId = params['id'];
      if (this._userId) {
        if (this._userId === this.userService.getActualUserId()) {
          this.router.navigate(['mygallery']);
        }
        this.loadingService.isLoading = true;
        if (params['gid']) {
          this.albumService.getAlbum(params['gid']).subscribe(album => {
            this._gallery = album;
          }).add(() => this.loadingService.isLoading = false);
        } else {
          this.userService.getUserProfile(this._userId).subscribe(profile => {
            this._profileData = profile;
            this.userService.getUserIsInRole(this._userId || '', 'admin').subscribe(admin => this._profileData!.role = admin ? 'Admin' : 'Regular')
          });
          this.albumService.getBrowsableAlbums(environment.default_page_size, environment.default_page).subscribe(albums => {
            this._galleries = albums.values;
            this._total = albums.total;
          }).add(() => this.loadingService.isLoading = false);;
        }
      }
    });
  }

  /**
   * Navigate to the details of the selected gallery
   * @param value Identity of the selected gallery
   */
  selectGallery(value: string) {
    this.router.navigate(['browse/user', this._userId, 'g', value]);
  }

  /**
   * Process pager change events
   * @param value Pager changed event
   */
  setPage(value: PagerModel) {
    this.albumService.getBrowsableAlbums(value.pageSize, value.page + 1).subscribe(albums => {
      this._galleries = albums.values;
      this._total = albums.total;
    });
  }

  /**
   * Navigate back to browse user
   */
  backToList() {
    this.router.navigate(['browse/user', this._userId]);
  }

  /**
   * Navigate back to browse
   */
  backToBrowse() {
    this.router.navigate(['browse']);
  }

  editUserRole(role: string) {
    this.loadingService.isLoading = true;
    this.userService.editUserRole({ id: this._userId || '', role })
    .subscribe(() => {
      this.snackService.openSnackBar(`User set as ${role}`, 'OK');
      this._profileData!.role = role;
    })
    .add(() => {
      this.loadingService.isLoading = false;
    });
  }

  addOrRemoveFavorite(g: AlbumViewModel | undefined, event: Event) {
    event.stopImmediatePropagation();
    this.confirmAlbumService.addOrRemoveFavorite(g)
      .add(() => {
        this.loadingService.isLoading = false;
        if (!g?.isFavorite) {
          this.snackService.openSnackBar('Removed from favorites!', 'OK');
        }
      });
  }

  likeGallery(g: AlbumViewModel | undefined, event: Event) {
    event.stopImmediatePropagation();
    this.confirmAlbumService.likeGallery(g)
      .add(() => {
        this._canLike = false;
        setTimeout(() => this._canLike = true, 5000)
        this.loadingService.isLoading = false;
      })
  }

  getImgClass(count: number) {
    if (count > 4) return 'sub-9';
    if (count > 1) return 'sub-4';
    return 'sub-1';
  }

  /**
   * Getter for user administrator status
   */
  get isAdmin(): boolean {
    return this.tokenService.getRole() === 'Admin';
  }

  /**
   * Getter for user administrator status
   */
  get isProfileAdmin(): boolean {
    return this._profileData?.role === 'Admin';
  }
  get profileData() { return this._profileData; }
  get galleries() { return this._galleries; }
  get gallery() { return this._gallery; }
  get total() { return this._total; }
  get placeholderImage() { return 'https://via.placeholder.com/100x100.png?text=Gallery'; }
  get userId() { return this.userService.getActualUserId(); }
  get canLike() { return this._canLike; }
}
