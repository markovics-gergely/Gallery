import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AlbumViewModel } from 'models';
import { LoadingService } from 'src/app/services/loading.service';

@Component({
  selector: 'app-my-galleries',
  templateUrl: './my-galleries.component.html',
  styleUrls: ['./my-galleries.component.scss']
})
export class MyGalleriesComponent implements OnInit {
  private _galleries: AlbumViewModel[] | undefined;
  private _selectedId: string | undefined;

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private loadingService: LoadingService
  ) { }

  ngOnInit(): void {
    this.loadingService.isLoading = true;
    this.route.params.subscribe(params => {
      if (params['id']) {
        this._selectedId = params['id'];
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

  selectGallery(value: string) {
    this.router.navigate(['mygalleries', value]);
  }

  setPageSize(value: number) {
    console.log(value);
  }

  setPage(value: number) {
    console.log(value);
  }

  get galleries() { return this._galleries; }
  get selectedId() { return this._selectedId; }
}
