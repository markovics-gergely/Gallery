import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { PagerModel } from 'models';

@Component({
  selector: 'app-pager',
  templateUrl: './pager.component.html',
  styleUrls: ['./pager.component.scss']
})
export class PagerComponent implements OnInit {
  /** Count of item of all pages */
  @Input() itemCount: number = 0;
  /** Event for user interaction */
  @Output() changeEvent = new EventEmitter<PagerModel>();
  /** Possible page sizes to choose from */
  private _pageSizes: number[] = [1, 5, 10, 20, 50, 100];
  /** Active page */
  private _page: number = 0;
  /** Selected page size */
  private _pageSize: number = 10;

  constructor() { }

  ngOnInit(): void {
    this.emitChange();
  }

  /**
   * Emit user interacted with pager 
   */
  private emitChange() {
    this.changeEvent.emit({
      page: this._page,
      pageSize: this._pageSize
    });
  }

  /**
   * Step to a neighboring page
   * @param upDown Flag of the way
   */
  stepPage(upDown: boolean) {
    this._page += upDown ? 1 : -1;
    this.emitChange();
  }

  /**
   * Step to specific page
   * @param page Page to step to
   */
  stepTo(page: number) {
    this._page = page;
    this.emitChange();
  }

  /**
   *  Step to the start or end of the pager
   * @param upDown Flag of the way
   */
  stepToEnd(upDown: boolean) {
    this._page = upDown ? this.pageCount - 1 : 0;
    this.emitChange();
  }

  /**
   * Check if page is at the end of the pager
   * @param upDown Flag of the way
   * @returns Flag for end
   */
  isAtEnd(upDown: boolean): boolean {
    return upDown ? (this._page === this.pageCount - 1) : (this._page === 0);
  }

  /**
   * Get sublist of the possible page numbers
   * @returns List of numbers to display
   */
  getPagerList(): number[] {
    let length = this.pageCount < 7 ? this.pageCount : 7;
    let start = this._page - 3 > 0 ? this._page - 3 : 0;
    if (start + length >= this.pageCount) {
      start = this.pageCount - length;
    }
    return Array.from({ length: length }, (_, k) => k + start);
  }

  /**
   * Change selected page size
   * @param event Size selection event
   */
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
