import { Component } from '@angular/core';
import { SidebarService } from './services/sidebar.service';
import { UserService } from './services/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Gallery Viewer';

  constructor(
    private userService: UserService,
    private sidebarService: SidebarService
  ) {}

  /**
   * Check if sidebar needs to be shown
   * @returns Flag of authentication
   */
  showSidebar(): boolean {
    return this.userService.isAuthenticated();
  }

  /**
   * Getter for sidebar status
   */
  get isSidebarOpen() { return this.sidebarService.isOpen; }
}
