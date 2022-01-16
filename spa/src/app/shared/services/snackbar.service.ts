import { ComponentType } from '@angular/cdk/portal';
import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable()
export class SnackbarService {
  constructor(private _snackBar: MatSnackBar) {}

  showError(message: string, title: string): void {
    this._snackBar.open(message, title, {
      horizontalPosition: 'end',
      verticalPosition: 'top',
    });
  }

  openWithComponent<T, K>(component: ComponentType<T>, data: K | null = null): void {
    this._snackBar.openFromComponent(component, {
      horizontalPosition: 'end',
      verticalPosition: 'top',
      data
    });
  }
}
