import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { TokenService } from '../services/token.service';
import { UserService } from '../services/user.service';

@Injectable({
  providedIn: 'root',
})
export class AdminGuard implements CanActivate {
  constructor(
    private userService: UserService,
    private router: Router,
    private token: TokenService
  ) {}

  /**
   * Send user to login page if there is no admin user logged in
   * @returns Flag of authentication
   */
  canActivate(): boolean | Promise<boolean> {
    let authenticated = this.userService.authenticated;
    if (!authenticated || !this.inRole) {
      this.router.navigate(['/login']);
    }
    return authenticated;
  }

  /**
   * Get if role property in the stored token equals admin role
   * @returns Flag for admin role
   */
  private get inRole(): boolean {
    return this.token.role === 'Admin';
  }
}
