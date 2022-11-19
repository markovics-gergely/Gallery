import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ProfileAlbumViewModel } from 'models';
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
}
