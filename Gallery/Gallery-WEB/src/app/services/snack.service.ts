import { Injectable } from '@angular/core';
import { MatSnackBar, MatSnackBarHorizontalPosition, MatSnackBarVerticalPosition } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class SnackService {
  private duration: number = 5;
  private hPosition: MatSnackBarHorizontalPosition = "end";
  private vPosition: MatSnackBarVerticalPosition = "bottom";

  constructor(private snackBar: MatSnackBar) { }

  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: this.duration * 1000,
      horizontalPosition: this.hPosition,
      verticalPosition: this.vPosition,
      panelClass: "snack-class"
    });
  }
}
