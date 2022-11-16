import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class SidebarService {
  private _isOpen: boolean = true;

  constructor(
    private router: Router
  ) {}

  /**
   * Getter for open status
   */
  get isOpen() { 
    if (['/login', '/register'].includes(this.router.url))
      return false;
    return this._isOpen;
  }

  /**
   * Setter for open status
   */
  set isOpen(value: boolean) { this._isOpen = value; }

  /**
   * Switch open status
   */
  switchOpen(): void {
    this._isOpen = !this._isOpen;
  }
}
