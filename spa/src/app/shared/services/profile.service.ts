import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, EMPTY, mapTo, Observable, of, switchMap, tap } from 'rxjs';
import { HttpService } from './http.service';
import { SnackbarService } from './snackbar.service';
import { ProfileRepository } from '../repository/profile.repository';

@Injectable({providedIn: 'root'})
export class ProfileService {
  constructor(
    private _http: HttpService,
    private _router: Router,
    private _profileRepo: ProfileRepository,
    private _snackBarService: SnackbarService
  ) {}

  loadCurrentProfile(): Observable<void> {
    return of(0).pipe(
      tap(() => {
        this._profileRepo.setLoading(true);
      }),
      switchMap(() => this._http.profileGetCurrent()),
      tap((resp) => {
        if (resp.isSuccess && resp.data != null) {
          this._profileRepo.setProfile(resp.data?.memberData ?? null);
        } else {
          let message = 'Произошла ошибка';
          if (resp.errors != null && resp.errors[0] != null) {
            message = resp.errors[0].message;
          }
          this._snackBarService.showError(message, 'Ошибка');
        }
        this._profileRepo.setLoading(false);
      }),
      catchError((err) => {
        this._profileRepo.setLoading(false);
        this._snackBarService.showError(err.Message, 'Ошибка');
        return EMPTY;
      }),
      mapTo(void 0)
    );
  }
}
