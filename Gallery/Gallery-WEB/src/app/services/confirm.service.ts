import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Observable } from 'rxjs';
import { ConfirmComponent } from '../components/dialogs/confirm/confirm.component';

@Injectable({
  providedIn: 'root',
})
export class ConfirmService {
  constructor(private dialog: MatDialog) {}

  /**
   * Create a confirmation dialog
   * @param title Title of the dialog
   * @param message Description text of the dialog
   * @returns Dialog result
   */
  public confirm(title: string, message: string): Observable<boolean> {
    const dialogRef = this.dialog.open(ConfirmComponent, {
      width: '40%',
      data: {
        title,
        message,
      },
    });
    return dialogRef.afterClosed();
  }
}
