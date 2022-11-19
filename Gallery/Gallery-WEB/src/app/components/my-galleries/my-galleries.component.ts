import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { AlbumDetailViewModel, AlbumViewModel, PagerModel } from 'models';
import { LoadingService } from 'src/app/services/loading.service';
import { AlbumDialogComponent } from '../dialogs/album-dialog/album-dialog.component';

@Component({
  selector: 'app-my-galleries',
  templateUrl: './my-galleries.component.html',
  styleUrls: ['./my-galleries.component.scss']
})
export class MyGalleriesComponent implements OnInit {
  private _galleries: AlbumViewModel[] | undefined;
  private _gallery: AlbumDetailViewModel | undefined;
  private _selectedId: string | undefined;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private loadingService: LoadingService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.loadingService.isLoading = true;
    this.route.params.subscribe(params => {
      if (params['id']) {
        this._selectedId = params['id'];
        this._gallery = {
          id: "JusticeForHarambe",
          name: "Rest in peace",
          creatorName: "Random kid",
          creatorId: "aefc41a9-37a5-467d-007f-08dac42e492a",
          isPrivate: true,
          isFavorite: true,
          likeCount: 15,
          pictureUrls: Array.from({ length: 20 }, (_, j) => (j % 2 == 0 ? 
            'https://www.gannett-cdn.com/-mm-/e922242eb72f53e3faf34889034c1a6ca9b2fe46/c=115-0-2595-3307/local/-/media/2016/05/31/Cincinnati/Cincinnati/636002970226066550-Harambe.jpg'
            : 'https://ichef.bbci.co.uk/news/976/cpsprodpb/D2D2/production/_118707935_untitled-1.jpg')
        )};
      } else {
        this._galleries = Array.from({ length: 20 }, (_, i) => ({
          id: "JusticeForHarambe",
          name: "Rest in peace",
          isPrivate: i % 2 == 0,
          isFavorite: i % 2 == 1,
          likeCount: i++,
          pictureUrls: Array.from({ length: 10 }, (_, j) => (j % 2 == 0 ? 
            'https://www.gannett-cdn.com/-mm-/e922242eb72f53e3faf34889034c1a6ca9b2fe46/c=115-0-2595-3307/local/-/media/2016/05/31/Cincinnati/Cincinnati/636002970226066550-Harambe.jpg'
            : 'https://ichef.bbci.co.uk/news/976/cpsprodpb/D2D2/production/_118707935_untitled-1.jpg')
        )}));
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
    console.log(value);
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
      console.log(result);
      if (result) {
        this.router.navigate(['mygalleries', result.id]);
      }
    });
  }

  /**
   * Navigate back to my gallery list
   */
  backToList() {
    this.router.navigate(['mygalleries']);
  }

  get galleries() { return this._galleries; }
  get gallery() { return this._gallery; }
  get selectedId() { return this._selectedId; }
}
