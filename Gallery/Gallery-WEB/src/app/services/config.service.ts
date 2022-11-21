import { Injectable } from '@angular/core';
import { ConfigViewModel } from 'models';

@Injectable({
  providedIn: 'root'
})
export class ConfigService {
  private _config: ConfigViewModel | undefined;

  constructor() { }
}
