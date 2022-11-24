import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { UserService } from '../services/user.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private userService: UserService, private router: Router) {}

  /**
   * Send user to login page if there is no user logged in
   * @returns Flag of authentication
   */
  canActivate(): boolean | Promise<boolean> {
    let authenticated = this.userService.authenticated;
    if (!authenticated) {
      this.router.navigate(['/login']);
    }
    return authenticated;
  }
}
