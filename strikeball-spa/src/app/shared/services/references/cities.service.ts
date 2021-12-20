import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, EMPTY, mapTo, Observable, of, switchMap, tap } from 'rxjs';
import { CitiesRepository } from '../../repository/cities.repository';
import { HttpService } from '../http.service';
import { SnackbarService } from '../snackbar.service';

@Injectable()
export class CitiesService {
  constructor(
    private _http: HttpService,
    private _router: Router,
    private _citiesRepo: CitiesRepository,
    private _snackBarService: SnackbarService
  ) {}

  loadCityReferences(): Observable<void> {
    return of(0).pipe(
      tap(() => {
        this._citiesRepo.setLoading(true);
      }),
      switchMap(() => this._http.loadCities()),
      tap((resp) => {
        if (resp.isSuccess && resp.data != null) {
          this._citiesRepo.upsertCities(resp.data?.regions ?? null);
        } else {
          let message = 'Произошла ошибка на подгрузке городов';
          if (resp.errors != null && resp.errors[0] != null) {
            message = resp.errors[0].message;
          }
          this._snackBarService.showError(message, 'Ошибка');
        }
        this._citiesRepo.setLoading(false);
      }),
      catchError((err) => {
        this._citiesRepo.setLoading(false);
        this._snackBarService.showError(err.Message || 'Подгрузка городов', 'Ошибка');
        return EMPTY;
      }),
      mapTo(void 0)
    );
  }
}
