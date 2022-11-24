import { Injectable } from '@angular/core';
import jwt_decode from 'jwt-decode';

@Injectable({
  providedIn: 'root',
})
export class TokenService {
  /**
   * Get token stored in the local storage
   */
  get accessToken(): string {
    let token = localStorage.getItem('accessToken');
    return token ? token : '';
  }

  /**
   * Get refresh token stored in the local storage
   */
  get refreshToken(): string {
    let token = localStorage.getItem('refreshToken');
    return token ? token : '';
  }

  /**
   * Set token stored in the local storage
   */
  set accessToken(accessToken: string) {
    localStorage.setItem('accessToken', accessToken);
  }

  /**
   * Set refresh token stored in the local storage
   */
  set refreshToken(refreshToken: string) {
    localStorage.setItem('refreshToken', refreshToken);
  }

  /**
   * Multisetter for local storage data
   * @param accessToken
   * @param refreshToken
   * @param username
   */
  public setLocalStorage(
    accessToken: string,
    refreshToken: string,
    username: string
  ) {
    localStorage.setItem('accessToken', accessToken);
    localStorage.setItem('refreshToken', refreshToken);
    localStorage.setItem('username', username);
  }

  /**
   * Clear data stored in local storage
   */
  deleteLocalStorage() {
    localStorage.clear();
  }

  /**
   * Get role of the user logged in from the local storage
   * @returns Role of the user
   */
  get role(): string {
    try {
      let jwt: { role: string } = jwt_decode(this.accessToken);
      return jwt.role;
    } catch (Error) {
      return '';
    }
  }
}
