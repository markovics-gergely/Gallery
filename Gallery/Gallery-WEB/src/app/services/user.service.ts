import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import {
  RegisterUserDTO,
  LoginUserDTO,
  ProfileViewModel,
  FullProfileViewModel,
  EditUserRoleDTO,
  UserMiniViewModel,
} from 'models';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  /** Route of the user related endpoints */
  private baseUrl: string = `${environment.baseUrl}/user`;

  constructor(private client: HttpClient, public jwtHelper: JwtHelperService) {}

  /**
   * Send registration request to the server
   * @param registerUserDTO User to registrate
   * @returns
   */
  public registration(registerUserDTO: RegisterUserDTO): Observable<Object> {
    return this.client.post(`${this.baseUrl}/registration`, registerUserDTO);
  }

  /**
   * Send login request to the server
   * @param loginUserDTO User to log in with
   * @returns Response from the server
   */
  public login(loginUserDTO: LoginUserDTO): Observable<Object> {
    let headers = new HttpHeaders().set(
      'Content-Type',
      'application/x-www-form-urlencoded'
    );
    let body = new URLSearchParams();

    body.set('username', loginUserDTO.username);
    body.set('password', loginUserDTO.password);
    body.set('grant_type', environment.grant_type);
    body.set('client_id', environment.client_id);
    body.set('client_secret', environment.client_secret);

    return this.client.post(
      `${environment.baseUrl}/connect/token`,
      body.toString(),
      { headers: headers }
    );
  }

  /**
   * Refresh stored token with refresh token
   * @param refreshToken Stored refresh token
   * @returns
   */
  public refresh(refreshToken: string): Observable<Object> {
    let headers = new HttpHeaders().set(
      'Content-Type',
      'application/x-www-form-urlencoded'
    );
    let body = new URLSearchParams();

    body.set('refresh_token', refreshToken);
    body.set('grant_type', 'refresh_token');
    body.set('client_id', environment.client_id);
    body.set('client_secret', environment.client_secret);

    return this.client.post(`${this.baseUrl}/refresh`, body.toString(), {
      headers: headers,
    });
  }

  /**
   * Edit role of a user
   * @param dto user to edit
   * @returns
   */
  public editUserRole(dto: EditUserRoleDTO): Observable<any> {
    return this.client.post<EditUserRoleDTO>(
      `${this.baseUrl}/role/change`,
      dto
    );
  }

  /**
   * Get identity of the user logged in
   */
  public get actualUserId(): string {
    const token = localStorage.getItem('accessToken');
    return this.jwtHelper.decodeToken(token ?? '')?.sub;
  }

  /**
   * Check if there is a user logged in
   * @returns Flag for login
   */
  public get authenticated(): boolean {
    const token = localStorage.getItem('accessToken');
    return token !== null && !this.jwtHelper.isTokenExpired(token);
  }

  /**
   * Get profile data of the user for the profile page
   * @returns Profile data of the user logged in
   */
  public getProfile(): Observable<ProfileViewModel> {
    return this.client.get<ProfileViewModel>(`${this.baseUrl}/profile`);
  }

  /**
   * Get detailed profile data of the user for the profile editor modal
   * @returns Detailed profile data of the user logged in
   */
  public getFullProfile(): Observable<FullProfileViewModel> {
    return this.client.get<FullProfileViewModel>(
      `${this.baseUrl}/full-profile`
    );
  }

  /**
   * Get detailed profile data of the user for the profile editor modal
   * @param id identity of the user
   * @returns Detailed profile data of the user
   */
  public getUserProfile(id: string): Observable<FullProfileViewModel> {
    if (this.authenticated) {
      return this.client.get<FullProfileViewModel>(
        `${this.baseUrl}/simple/${id}`
      );
    }
    return of();
  }

  /**
   * Get list of users with the role specified
   * @param role role to search with
   * @returns List of users
   */
  public getUsersByRole(role: string): Observable<UserMiniViewModel[]> {
    return this.client.get<UserMiniViewModel[]>(`${this.baseUrl}/role/${role}`);
  }

  /**
   *
   * @param id Identity of the user
   * @param role Role to check
   * @returns Flag for the role
   */
  public getUserIsInRole(id: string, role: string): Observable<boolean> {
    if (this.authenticated) {
      return this.getUsersByRole(role).pipe(
        map((users) => users.some((u) => u.id === id))
      );
    }
    return of(false);
  }
}
