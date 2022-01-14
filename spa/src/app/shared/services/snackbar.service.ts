import { ComponentType } from '@angular/cdk/portal';
import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({ providedIn: 'root' })
export class SnackbarService {
  constructor(private _snackBar: MatSnackBar) {}

  showError(message: string, title: string): void {
    this._snackBar.open(message, title, {
      horizontalPosition: 'end',
      verticalPosition: 'top',
    });
  }

  openWithComponent<T>(component: ComponentType<T>): void {
    this._snackBar.openFromComponent(component, {
      horizontalPosition: 'end',
      verticalPosition: 'top',
    });
  }
}
