import { Injectable } from '@angular/core';
import { AlbumViewModel } from 'models';
import { AlbumService } from './album.service';
import { ConfirmService } from './confirm.service';
import { LoadingService } from './loading.service';

@Injectable({
  providedIn: 'root'
})
export class ConfirmAlbumService {

  constructor(
    private albumService: AlbumService,
    private confirmService: ConfirmService,
    private loadingService: LoadingService
  ) { }

  addOrRemoveFavorite(gallery: AlbumViewModel | undefined) {
    if (gallery?.isFavorite) {
      this.confirmService.confirm(
        "Add to favorite",
        `Are you sure you want to remove ${gallery?.name} from your favorites?`
      ).subscribe(res => {
        if (res) {
          this.loadingService.isLoading = true;
          this.albumService.removeFavorites(gallery?.id || '')
            .subscribe(_ => gallery.isFavorite = false).add(() => this.loadingService.isLoading = false);
        }
      });
    } else {
      this.loadingService.isLoading = true;
      this.albumService.addToFavorites(gallery?.id || '')
        .subscribe(_ => gallery!.isFavorite = true).add(() => this.loadingService.isLoading = false);
    }
  }

  setPublicStatus(gallery: AlbumViewModel | undefined) {
    this.confirmService.confirm(
      "Change album privacy",
      `Are you sure you want to set the gallery ${gallery?.isPrivate ? 'public' : 'private'}?`
    ).subscribe(res => {
      if (res) {
        this.loadingService.isLoading = true;
        this.albumService.editAlbum({
          name: gallery?.name || '',
          isPrivate: !gallery?.isPrivate
        }, gallery?.id || '')
          .subscribe(_ => gallery!.isPrivate = !gallery?.isPrivate)
          .add(() => this.loadingService.isLoading = false);
      }
    });
  }

  deleteGallery(gallery: AlbumViewModel | undefined) {
    return this.confirmService.confirm(
      "Delete gallery",
      `Are you sure you want to delete ${gallery?.name}?`
    ).subscribe(res => {
      if (res) {
        this.loadingService.isLoading = true;
        return this.albumService.deleteAlbum(gallery?.id || '')
          .subscribe().add(() => this.loadingService.isLoading = false);
      }
    });
  }
}
