import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PagerModel, ProfileAlbumViewModel, ProfileViewModel } from 'models';
import { AlbumService } from 'src/app/services/album.service';
import { LoadingService } from 'src/app/services/loading.service';
import { TokenService } from 'src/app/services/token.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  private _profileData: ProfileViewModel | undefined;
  private _albums: ProfileAlbumViewModel[] | undefined;
  private _total: number = 0;

  constructor(
    private userService: UserService,
    private router: Router,
    private loadingService: LoadingService,
    private tokenService: TokenService,
    private albumService: AlbumService
  ) { }

  ngOnInit(): void {
    this.loadingService.isLoading = true;
    this.userService.getProfile().subscribe(data => {
      this._profileData = data;
      this.albumService.getProfileAlbums(10, 1).subscribe(albums => {
        this._albums = albums.values;
        this._total = albums.total;
        this.loadingService.isLoading = false;
      });
    });
  }

  /**
   * Process pager change events
   * @param value Pager changed event
   */
  setPage(value: PagerModel) {
    this.albumService.getProfileAlbums(value.pageSize, value.page + 1).subscribe(albums => {
      this._albums = albums.values;
      this._total = albums.total;
    });
  }

  /**
   * Open profile editor modal
   */
  openEdit() {

  }

  /**
   * Navigate to the details page of the chosen gallery
   * @param id Identity of the chosen gallery
   */
  navigateToGallery(id: string) {
    this.router.navigate(['/mygalleries', id]);
  }

  /**
   * Getter for user administrator status
   */
  get isAdmin(): boolean {
    return this.tokenService.getRole() === 'Admin';
  }
  get profileData() { return this._profileData; }
  get albums() { return this._albums; }
  get total() { return this._total; }
  get placeholderImage() { return 'https://via.placeholder.com/150x120.png?text=Gallery'; }
}
