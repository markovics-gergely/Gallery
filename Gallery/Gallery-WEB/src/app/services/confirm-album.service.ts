import { Injectable } from '@angular/core';
import { AddAlbumPicturesDTO, AlbumViewModel, RemoveAlbumPicturesDTO } from 'models';
import { AlbumService } from './album.service';
import { ConfirmService } from './confirm.service';
import { LoadingService } from './loading.service';
import { SnackService } from './snack.service';

@Injectable({
  providedIn: 'root',
})
export class ConfirmAlbumService {
  constructor(
    private albumService: AlbumService,
    private confirmService: ConfirmService,
    private loadingService: LoadingService,
    private snackService: SnackService
  ) {}

  addOrRemoveFavorite(gallery: AlbumViewModel | undefined) {
    if (gallery?.isFavorite) {
      return this.confirmService
        .confirm(
          'Add to favorite',
          `Are you sure you want to remove ${gallery?.name} from your favorites?`
        )
        .subscribe((res) => {
          if (res) {
            this.loadingService.isLoading = true;
            this.albumService
              .removeFavorites(gallery?.id || '')
              .subscribe((_) => {
                gallery.isFavorite = false;
                gallery.likeCount!--;
              });
          }
        });
    } else {
      this.loadingService.isLoading = true;
      return this.albumService
        .addToFavorites(gallery?.id || '')
        .subscribe((_) => {
          gallery!.isFavorite = true;
          gallery!.likeCount!++;
          this.snackService.openSnackBar('Added to favorites!', 'OK');
        });
    }
  }

  setPublicStatus(gallery: AlbumViewModel | undefined) {
    return this.confirmService
      .confirm(
        'Change album privacy',
        `Are you sure you want to set the gallery ${
          gallery?.isPrivate ? 'public' : 'private'
        }?`
      )
      .subscribe((res) => {
        if (res) {
          this.loadingService.isLoading = true;
          this.albumService
            .editAlbum(
              {
                name: gallery?.name || '',
                isPrivate: !gallery?.isPrivate,
              },
              gallery?.id || ''
            )
            .subscribe((_) => {
              gallery!.isPrivate = !gallery?.isPrivate;
              this.snackService.openSnackBar(
                `Set to ${gallery?.isPrivate ? 'private' : 'public'}`,
                'OK'
              );
            })
            .add(() => {
              this.loadingService.isLoading = false;
            });
        }
      });
  }

  deleteGallery(gallery: AlbumViewModel | undefined) {
    return this.confirmService
      .confirm(
        'Delete gallery',
        `Are you sure you want to delete ${gallery?.name}?`
      )
      .subscribe((res) => {
        if (res) {
          this.loadingService.isLoading = true;
          return this.albumService
            .deleteAlbum(gallery?.id || '')
            .subscribe(() => {
              this.snackService.openSnackBar('Gallery deleted', 'OK');
            })
            .add(() => (this.loadingService.isLoading = false));
        }
      });
  }

  likeGallery(gallery: AlbumViewModel | undefined) {
    this.loadingService.isLoading = true;
    return this.albumService
      .likeAlbum(gallery?.id || '')
      .subscribe(() => {
        gallery!.likeCount!++;
        this.snackService.openSnackBar(`You liked ${gallery?.name}`, 'OK');
      });
  }

  addPictures(id: string, dto: AddAlbumPicturesDTO) {
    this.loadingService.isLoading = true;
    return this.albumService
      .addPictures(id, dto)
      .subscribe(() => {
        this.snackService.openSnackBar(`${dto.pictures.length} Pictures added`, 'OK');
      })
      .add(() => this.loadingService.isLoading = false);
  }

  /**
   * Delete the images from the gallery
   * @param id Identity of the gallery
   * @param dto Pictures to delete
   */
   deletePictures(id: string, dto: RemoveAlbumPicturesDTO) {
    return this.confirmService.confirm(
      "Delete picture",
      "Are you sure you want to delete this picture?"
    ).subscribe((res) => {
      this.loadingService.isLoading = true;
      if (res) {
        this.albumService.removePictures(id, dto)
          .subscribe(() => {
            if (dto.PictureIds.length > 1) {
              this.snackService.openSnackBar(`${dto.PictureIds.length} Pictures removed`, 'OK');
            } else {
              this.snackService.openSnackBar(`Picture removed`, 'OK');
            }
          })
          .add(() => this.loadingService.isLoading = false);
      }
    });
  }
}
