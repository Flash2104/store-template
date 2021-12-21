import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, EMPTY, mapTo, Observable, of, switchMap, tap } from 'rxjs';
import { NavigationRepository } from '../repository/navigation.repository';
import { HttpService } from './http.service';
import { SnackbarService } from './snackbar.service';

@Injectable({ providedIn: 'root' })
export class NavigationService {
  constructor(
    private _http: HttpService,
    private _router: Router,
    private _navRepo: NavigationRepository,
    private _snackBarService: SnackbarService
  ) {}

  loadUserNavigation(): Observable<void> {
    return of(0).pipe(
      tap(() => {
        this._navRepo.setLoading(true);
      }),
      switchMap(() => this._http.getUserNavigation()),
      tap((resp) => {
        if (resp.isSuccess && resp.data != null) {
          this._navRepo.setData(resp.data.navigations ?? null);
        } else {
          let message = 'Произошла ошибка';
          if (resp.errors != null && resp.errors[0] != null) {
            message = resp.errors[0].message;
          }
          this._snackBarService.showError(message, 'Ошибка');
        }
        this._navRepo.setLoading(false);
      }),
      catchError((err) => {
        this._navRepo.setLoading(false);
        this._snackBarService.showError(err.Message, 'Ошибка');
        return EMPTY;
      }),
      mapTo(void 0)
    );
  }
}
