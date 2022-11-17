import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-pager',
  templateUrl: './pager.component.html',
  styleUrls: ['./pager.component.scss']
})
export class PagerComponent implements OnInit {
  @Input() pageCount: number = 0;
  @Output() pageEvent = new EventEmitter<number>();
  @Output() sizeEvent = new EventEmitter<number>();
  private _pageSizes: number[] = [5, 10, 20, 50, 100];
  private _page: number = 0;
  private _pageSize: number = 10;

  constructor() { }

  ngOnInit(): void {
    this.sizeEvent.emit(this._pageSize);
    this.pageEvent.emit(this._page);
  }

  stepPage(upDown: boolean) {
    this._page += upDown ? 1 : -1;
    this.pageEvent.emit(this._page);
  }

  stepTo(page: number) {
    this._page = page;
    this.pageEvent.emit(this._page);
  }

  stepToEnd(upDown: boolean) {
    this._page = upDown ? this.pageCount - 1 : 0;
    this.pageEvent.emit(this._page);
  }

  isAtEnd(upDown: boolean): boolean {
    return upDown ? (this._page === this.pageCount - 1) : (this._page === 0);
  }

  getPagerList(): number[] {
    let length = this.pageCount < 7 ? this.pageCount : 7;
    let start = this._page - 3 > 0 ? this._page - 3 : 0;
    if (start + length >= this.pageCount) {
      start = this.pageCount - length;
    }
    return Array.from({ length: length }, (_, k) => k + start);
  }

  changeSize(event: Event) {
    if (typeof event === "number") {
      this._pageSize = event;
      this.sizeEvent.emit(this._pageSize);
    }
  }

  get pageSizes(): number[] { return this._pageSizes; }
  get pageSize(): number { return this._pageSize; }
  get page(): number { return this._page; }
}
