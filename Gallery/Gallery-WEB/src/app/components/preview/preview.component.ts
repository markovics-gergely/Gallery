import { Component, OnInit } from '@angular/core';
import { PreviewService } from 'src/app/services/preview.service';

@Component({
  selector: 'app-preview',
  templateUrl: './preview.component.html',
  styleUrls: ['./preview.component.scss']
})
export class PreviewComponent implements OnInit {

  constructor(
    private previewService: PreviewService
  ) { }

  ngOnInit(): void {
  }

  close() {
    this.previewService.previewImage = undefined;
  }

  get previewImage() { return this.previewService.previewImage; }
}
