import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { RegisterUserDTO, LoginUserDTO } from 'models';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  /** Route of the user related endpoints */
  private baseUrl: string = `${environment.baseUrl}/user`;

  constructor(
    private client: HttpClient,
    public jwtHelper: JwtHelperService
  ) { }

  /**
   * Send registration request to the server
   * @param registerUserDTO User to registrate
   * @returns 
   */
  public registration(registerUserDTO: RegisterUserDTO): Observable<Object> {
    return this.client.post(`${this.baseUrl}/registration`, registerUserDTO)
  }

  /**
   * Send login request to the server
   * @param loginUserDTO User to log in with
   * @returns Response from the server
   */
  public login(loginUserDTO: LoginUserDTO): Observable<Object> {
    let headers = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
    let body = new URLSearchParams();

    body.set('username', loginUserDTO.username);
    body.set('password', loginUserDTO.password);
    body.set('grant_type', environment.grant_type);
    body.set('client_id', environment.client_id);
    body.set('client_secret', environment.client_secret);

    return this.client.post(`${this.baseUrl}/login`, body.toString(), { headers: headers })
  }

  /**
   * Refresh stored token with refresh token
   * @param refreshToken Stored refresh token
   * @returns 
   */
  public refresh(refreshToken: string): Observable<Object> {
    let headers = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');
    let body = new URLSearchParams();

    body.set('refresh_token', refreshToken);
    body.set('grant_type', "refresh_token");
    body.set('client_id', environment.client_id);
    body.set('client_secret', environment.client_secret);

    return this.client.post(`${this.baseUrl}/refresh`, body.toString(), { headers: headers })
  }

  /**
   * Get identity of the user logged in
   * @returns Id of the user
   */
  public getActualUserId(): Observable<string> {
    return this.client.get(`${this.baseUrl}/actual-id`, {responseType: 'text'});
  }

  /**
   * Check if there is a user logged in
   * @returns Flag for login
   */
  public isAuthenticated(): boolean {
    const token = localStorage.getItem('accessToken');
    return token !== null && !this.jwtHelper.isTokenExpired(token);
  }
}
