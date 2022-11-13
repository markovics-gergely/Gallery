import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { UserService } from '../services/user.service';

@Injectable({
  providedIn: 'root'
})
export class LoginGuard implements CanActivate {
  constructor(
    private userService: UserService,
    private router: Router
  ) {}

  /**
   * Skip authentication if there is a user logged in
   * @returns Flag of authentication
   */
  canActivate(): boolean | Promise<boolean> {
    let isAuthenticated = this.userService.isAuthenticated();
    if (isAuthenticated) {
      this.router.navigate(['/profile']);
    }
    return true;
  }
}
