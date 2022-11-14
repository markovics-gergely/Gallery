import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
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
    private tokenService: TokenService
  ) { }

  ngOnInit(): void {
  }

  /**
   * Getter for open status
   */
  get isOpen() { return this._isOpen; }

  /**
   * Switch open status
   */
  switchOpen(): void {
    this._isOpen = !this._isOpen;
  }

  /**
   * Get current active route
   */
  get activeMenu() {
    return this.router.url.slice(1);
  }

  /**
   * Remove user from storage
   */
  logout(): void {
    this.tokenService.deleteLocalStorage();
  }
}
