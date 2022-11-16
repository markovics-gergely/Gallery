import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { SidebarService } from 'src/app/services/sidebar.service';
import { TokenService } from 'src/app/services/token.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {
  private _isOpen: boolean = true;

  constructor(
    private router: Router,
    private tokenService: TokenService,
    private sidebarService: SidebarService
  ) { }

  ngOnInit(): void {
  }

  /**
   * Getter for open status
   */
  get isOpen() { return this.sidebarService.isOpen; }

  /**
   * Switch open status
   */
  switchOpen(): void {
    this.sidebarService.switchOpen();
  }

  /**
   * Get current active route
   */
  get activeMenu() {
    return this.router.url.slice(1).split('/')[0];
  }

  /**
   * Remove user from storage
   */
  logout(): void {
    this.tokenService.deleteLocalStorage();
  }
}
