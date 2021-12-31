import { ShopRepository } from './../repository/shop.repository';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, EMPTY, mapTo, Observable, of, switchMap, tap } from 'rxjs';
import { HttpService } from './http.service';
import { SnackbarService } from './snackbar.service';

@Injectable({ providedIn: 'root' })
export class ShopService {
  constructor(
    private _http: HttpService,
    private _router: Router,
    private _shopRepo: ShopRepository,
    private _snackBarService: SnackbarService
  ) {}

  loadShopInfo(): Observable<void> {
    return of(0).pipe(
      tap(() => {
        this._shopRepo.setLoading(true);
      }),
      switchMap(() => this._http.getStoreInfo()),
      tap((resp) => {
        if (resp.isSuccess && resp.data != null) {
          this._shopRepo.setShopData(resp.data ?? null);
        } else {
          let message = 'Произошла ошибка';
          if (resp.errors != null && resp.errors[0] != null) {
            message = resp.errors[0].message;
          }
          this._snackBarService.showError(message, 'Ошибка');
        }
        this._shopRepo.setLoading(false);
      }),
      catchError((err) => {
        this._shopRepo.setLoading(false);
        this._snackBarService.showError(err.Message, 'Ошибка');
        return EMPTY;
      }),
      mapTo(void 0)
    );
  }
}
