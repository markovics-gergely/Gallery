import { TestBed } from '@angular/core/testing';

import { ConfirmAlbumService } from './confirm-album.service';

describe('ConfirmAlbumService', () => {
  let service: ConfirmAlbumService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ConfirmAlbumService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
