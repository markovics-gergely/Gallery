import { Injectable } from '@angular/core';
import { AlbumDetailViewModel } from 'models';
import { AlbumService } from './album.service';
import { ConfirmService } from './confirm.service';

@Injectable({
  providedIn: 'root'
})
export class ConfirmAlbumService {

  constructor(
    private albumService: AlbumService,
    private confirmService: ConfirmService
  ) { }

  addOrRemoveFavorite(gallery: AlbumDetailViewModel | undefined) {
    if (gallery?.isFavorite) {
      this.confirmService.confirm(
        "Add to favorite",
        `Are you sure you want to remove ${gallery?.name} from your favorites?`
      ).subscribe(res => {
        if (res) {
          
        }
      });
    } else {
      this.albumService.addToFavorites({ albumIds: [gallery?.id || '']})
    }
  }

  setPublicStatus(gallery: AlbumDetailViewModel | undefined) {
    this.confirmService.confirm(
      "Change album privacy",
      `Are you sure you want to set the gallery ${gallery?.isPrivate ? 'public' : 'private'}?`
    ).subscribe(res => {
      if (res) {
        if (gallery?.isPrivate) {
      
        } else {

        }
      }
    });
  }

  deleteGallery(gallery: AlbumDetailViewModel | undefined) {
    this.confirmService.confirm(
      "Delete gallery",
      `Are you sure you want to delete ${gallery?.name}?`
    ).subscribe(res => {
      if (res) {
        this.albumService.deleteAlbum(gallery?.id || '');
      }
    });
  }
}
