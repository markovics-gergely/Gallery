import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ProfileAlbumViewModel, ProfileViewModel } from 'models';
import { LoadingService } from 'src/app/services/loading.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  private _profileData: ProfileViewModel | undefined;
  private _albums: ProfileAlbumViewModel[] | undefined;

  constructor(
    private userService: UserService,
    private router: Router,
    private loadingService: LoadingService
  ) { }

  ngOnInit(): void {
    this.loadingService.isLoading = true;
    this.userService.getProfile().subscribe(data => {
      this._profileData = data;
      this._albums = new Array(20).fill({
        id: "JusticeForHarambe",
        name: "Rest in peace",
        countOfPictures: 10,
        coverPicture: {
          path: 'https://www.gannett-cdn.com/-mm-/e922242eb72f53e3faf34889034c1a6ca9b2fe46/c=115-0-2595-3307/local/-/media/2016/05/31/Cincinnati/Cincinnati/636002970226066550-Harambe.jpg',
          fileExtension: 'png'
        }
      });
      this.loadingService.isLoading = false;
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

  get profileData() { return this._profileData; }
  get albums() { return this._albums; }
}
