import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { SidebarService } from './services/sidebar.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Gallery Viewer';

  constructor(
    private sidebarService: SidebarService,
    private router: Router
  ) {}

  /**
   * Check if sidebar needs to be shown
   * @returns Flag of authentication
   */
  showSidebar(): boolean {
    return !['/login', '/register'].includes(this.router.url);
  }

  /**
   * Getter for sidebar status
   */
  get sidebarOpen() { return this.sidebarService.isOpen; }
}
