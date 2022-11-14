import { Component } from '@angular/core';
import { UserService } from './services/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Gallery Viewer';

  constructor(
    private userService: UserService
  ) {}

  /**
   * Check if sidebar needs to be shown
   * @returns Flag of authentication
   */
  showSidebar(): boolean {
    return this.userService.isAuthenticated();
  }
}
