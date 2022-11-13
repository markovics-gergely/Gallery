import { Injectable } from '@angular/core';
import jwt_decode from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class TokenService {

  constructor() { }

  getAccessToken(): string {
    let token = localStorage.getItem('accessToken');
    return token ? token : "";
  }

  getRefreshToken(): string {
    let token = localStorage.getItem('refreshToken');
    return token ? token : "";
  }

  setAccessToken(accessToken: string) { localStorage.setItem('accessToken', accessToken); }

  setRefreshToken(refreshToken: string) { localStorage.setItem('refreshToken', refreshToken); }

  setLocalStorage(accessToken: string, refreshToken: string, username: string) {
    localStorage.setItem('accessToken', accessToken);
    localStorage.setItem('refreshToken', refreshToken);
    localStorage.setItem('username', username);
  }

  deleteLocalStorage() { localStorage.clear(); }

  getRole(): string{
    try {
      let jwt: { role: string } = jwt_decode(this.getAccessToken());
      return jwt.role;
    } catch(Error) {
      return "";
    }
  }
}
