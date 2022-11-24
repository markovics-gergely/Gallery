import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {
  AddAlbumPicturesDTO,
  AlbumViewModel,
  ConfigViewModel,
  CreateAlbumDTO,
  EditAlbumDTO,
  PagerList,
  ProfileAlbumViewModel,
  RemoveAlbumPicturesDTO,
} from 'models';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root',
})
export class AlbumService {
  /** Base url of album endpoint */
  private _baseUrl = `${environment.baseUrl}/album`;

  constructor(private http: HttpClient, private userService: UserService) {}

  /**
   * Send get request for albums for the profile page
   * @returns List of album data for profile page
   */
  public getProfileAlbums(
    pageSize: number,
    pageCount: number
  ): Observable<PagerList<ProfileAlbumViewModel>> {
    return this.http.get<PagerList<ProfileAlbumViewModel>>(
      `${this._baseUrl}/user`,
      {
        params: new HttpParams()
          .set('PageSize', pageSize)
          .set('PageCount', pageCount),
      }
    );
  }

  /**
   * Send get request for albums for the list page
   * @returns List of album data for list page
   */
  public getAlbums(
    pageSize: number,
    pageCount: number,
    user: string | undefined
  ): Observable<PagerList<AlbumViewModel>> {
    return this.http.get<PagerList<AlbumViewModel>>(
      `${this._baseUrl}/user/${user || this.userService.actualUserId}`,
      {
        params: new HttpParams()
          .set('PageSize', pageSize)
          .set('PageCount', pageCount),
      }
    );
  }

  /**
   * Send get request for albums for the favorite page
   * @returns List of album data for favorite page
   */
  public getFavoriteAlbums(
    pageSize: number,
    pageCount: number
  ): Observable<PagerList<AlbumViewModel>> {
    return this.http.get<PagerList<AlbumViewModel>>(
      `${this._baseUrl}/favorites`,
      {
        params: new HttpParams()
          .set('PageSize', pageSize)
          .set('PageCount', pageCount),
      }
    );
  }

  /**
   * Send get request for albums for the browse page
   * @returns List of album data for browse page
   */
  public getBrowsableAlbums(
    pageSize: number,
    pageCount: number
  ): Observable<PagerList<AlbumViewModel>> {
    return this.http.get<PagerList<AlbumViewModel>>(this._baseUrl, {
      params: new HttpParams()
        .set('PageSize', pageSize)
        .set('PageCount', pageCount),
    });
  }

  /**
   * Send get request for album details
   * @returns Album with detailed informations
   */
  public getAlbum(albumId: string): Observable<AlbumViewModel> {
    return this.http.get<AlbumViewModel>(`${this._baseUrl}/${albumId}`);
  }

  /**
   * Send album create request
   * @param dto Album to create
   * @returns Album identity created
   */
  public createAlbum(dto: CreateAlbumDTO): Observable<boolean> {
    return this.http.post<boolean>(this._baseUrl, this.getFormData(dto));
  }

  /**
   * Send album edit request
   * @param dto Album to edit
   * @returns Request result
   */
  public editAlbum(dto: EditAlbumDTO, albumId: string): Observable<any> {
    console.log(dto);

    return this.http.put(`${this._baseUrl}/${albumId}`, dto);
  }

  /**
   * Send album delete request
   * @param id Identity of album to delete
   * @returns Request result
   */
  public deleteAlbum(id: string): Observable<any> {
    return this.http.delete(`${this._baseUrl}/${id}`);
  }

  /**
   * Send a like album request
   * @param id Identity of album to like
   * @returns Request result
   */
  public likeAlbum(id: string): Observable<any> {
    return this.http.post(`${this._baseUrl}/${id}/like`, {});
  }

  /**
   * Add pictures to an album request
   * @param id Identity of album to add to
   * @param dto Pictures to add
   * @returns Request result
   */
  public addPictures(id: string, dto: AddAlbumPicturesDTO): Observable<any> {
    return this.http.put(
      `${this._baseUrl}/${id}/pictures/add`,
      this.getFormData(dto)
    );
  }

  /**
   * Remove pictures from an album request
   * @param id Identity of album to remove from
   * @param dto Pictures to remove
   * @returns Request result
   */
  public removePictures(
    id: string,
    dto: RemoveAlbumPicturesDTO
  ): Observable<any> {
    return this.http.put(`${this._baseUrl}/${id}/pictures/remove`, dto);
  }

  /**
   * Add an album to favorites request
   * @param albumId Identity of album to add
   * @returns Request result
   */
  public addToFavorites(albumId: string): Observable<any> {
    return this.http.post(`${this._baseUrl}/favorites/${albumId}`, {});
  }

  /**
   * Remove an album from favorites request
   * @param albumId Identity of album to remove
   * @returns Request result
   */
  public removeFavorites(albumId: string): Observable<any> {
    return this.http.delete(`${this._baseUrl}/favorites/${albumId}`, {});
  }

  /**
   * Load Config data from the server
   * @returns Config data for albums
   */
  public loadConfig(): Observable<ConfigViewModel> {
    return this.http.get<ConfigViewModel>(`${this._baseUrl}/config`);
  }

  /**
   * Generate form data from object
   * @param obj Object to transform
   * @returns FormData generated
   */
  private getFormData(obj: any): FormData {
    return Object.keys(obj).reduce((formData, key) => {
      if (key === 'pictures') {
        (obj[key] as File[]).forEach((file) => {
          formData.append(key, file);
        });
      } else {
        formData.append(key, obj[key]);
      }
      return formData;
    }, new FormData());
  }
}
