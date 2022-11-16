import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-my-galleries',
  templateUrl: './my-galleries.component.html',
  styleUrls: ['./my-galleries.component.scss']
})
export class MyGalleriesComponent implements OnInit {
  private _galleryId: string | undefined;

  constructor(
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this._galleryId = params['id'];
    })
  }

  get galleryId() { return this._galleryId; }
}
