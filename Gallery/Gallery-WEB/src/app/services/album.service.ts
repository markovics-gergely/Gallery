import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AddAlbumPicturesDTO, CreateAlbumDTO, EditFavoritesDTO, ProfileAlbumViewModel, RemoveAlbumPicturesDTO } from 'models';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class AlbumService {
  /** Base url of album endpoint */
  private _baseUrl = `${environment.baseUrl}/album`;

  constructor(
    private http: HttpClient,
    private userService: UserService
  ) { }

  public getProfileAlbums(): Observable<ProfileAlbumViewModel[]> {
    return this.http.get<ProfileAlbumViewModel[]>(`${this._baseUrl}/${this.userService.getActualUserId()}`)
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
   * Send album delete request
   * @param id Identity of album to delete
   * @returns
   */
  public deleteAlbum(id: string): Observable<any> {
    return this.http.delete(`${this._baseUrl}/${id}`);
  }

  /**
   * Send a like album request
   * @param id Identity of album to like
   * @returns
   */
  public likeAlbum(id: string): Observable<any> {
    return this.http.post(`${this._baseUrl}/${id}/like`, {});
  }

  /**
   * Add pictures to an album request
   * @param id Identity of album to add to
   * @param dto Pictures to add
   * @returns
   */
  public addPictures(id: string, dto: AddAlbumPicturesDTO): Observable<any> {
    return this.http.put(`${this._baseUrl}/${id}/pictures/add`, dto);
  }

  /**
   * Remove pictures from an album request
   * @param id Identity of album to remove from
   * @param dto Pictures to remove
   * @returns
   */
  public removePictures(id: string, dto: RemoveAlbumPicturesDTO): Observable<any> {
    return this.http.put(`${this._baseUrl}/${id}/pictures/remove`, dto);
  }

  /**
   * Add an album to favorites request
   * @param dto Identity of album to add
   * @returns
   */
  public addToFavorites(dto: EditFavoritesDTO): Observable<any> {
    return this.http.put(`${this._baseUrl}/favorites`, dto);
  }

  /**
   * Generate form data from object
   * @param obj Object to transform
   * @returns FormData generated
   */
  private getFormData(obj: any): FormData {
    return Object.keys(obj).reduce((formData, key) => {
      formData.append(key, obj[key]);
      return formData;
    }, new FormData());
  }
}
