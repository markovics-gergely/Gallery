import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { PagerModel } from 'models';

@Component({
  selector: 'app-pager',
  templateUrl: './pager.component.html',
  styleUrls: ['./pager.component.scss']
})
export class PagerComponent implements OnInit {
  @Input() itemCount: number = 0;
  @Output() changeEvent = new EventEmitter<PagerModel>();
  private _pageSizes: number[] = [1, 5, 10, 20, 50, 100];
  private _page: number = 0;
  private _pageSize: number = 10;

  constructor() { }

  ngOnInit(): void {
    this.emitChange();
  }

  private emitChange() {
    this.changeEvent.emit({
      page: this._page,
      pageSize: this._pageSize
    });
  }

  stepPage(upDown: boolean) {
    this._page += upDown ? 1 : -1;
    this.emitChange();
  }

  stepTo(page: number) {
    this._page = page;
    this.emitChange();
  }

  stepToEnd(upDown: boolean) {
    this._page = upDown ? this.pageCount - 1 : 0;
    this.emitChange();
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
      this.emitChange();
    }
  }

  get pageCount(): number { return Math.ceil(this.itemCount / this._pageSize); }
  get pageSizes(): number[] { return this._pageSizes; }
  get pageSize(): number { return this._pageSize; }
  get page(): number { return this._page; }
}
