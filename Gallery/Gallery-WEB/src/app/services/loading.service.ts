import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  private _isLoading: boolean = false;

  get isLoading() { return this._isLoading; }
  set isLoading(value: boolean) { this._isLoading = value; }
}
