import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import {
  catchError,
  combineLatest,
  EMPTY,
  map,
  mapTo,
  Observable,
  of,
  switchMap,
  take,
  tap,
} from 'rxjs';
import { HttpService } from './http.service';
import { SnackbarService } from './snackbar.service';
import { AuthRepository } from '../repository/auth.repository';
import { ISignInData } from '../../public/auth/auth-container/sign-in/sign-in.component';
import { ISignUpData } from '../../public/auth/auth-container/sign-up/sign-up.component';

@Injectable({ providedIn: 'root' })
export class AuthService {
  constructor(
    private _http: HttpService,
    private _router: Router,
    private _authRepo: AuthRepository,
    private _snackBarService: SnackbarService
  ) {}

  signIn(data: ISignInData): Observable<void> {
    return of(0).pipe(
      tap(() => {
        this._authRepo.setLoading(true);
      }),
      switchMap(() => this._http.authSignIn(data)),
      tap((resp) => {
        if (resp.isSuccess && resp.data?.tokenData?.token != null) {
          this._authRepo.updateUserToken(
            resp.data?.user ?? null,
            resp.data?.tokenData ?? null
          );
          this._router.navigate(['private', 'profile']).then((res) => {
            this._authRepo.setLoading(false);
          });
        } else {
          let message = 'Произошла ошибка';
          if (resp.errors != null && resp.errors[0] != null) {
            message = resp.errors[0].message;
          }
          this._snackBarService.showError(message, 'Ошибка');
        }
        this._authRepo.setLoading(false);
      }),
      catchError((err) => {
        this._authRepo.setLoading(false);
        this._snackBarService.showError(err.Message, 'Ошибка');
        return EMPTY;
      }),
      mapTo(void 0)
    );
  }

  signUp(data: ISignUpData): Observable<void> {
    return of(0).pipe(
      tap(() => {
        this._authRepo.setLoading(true);
      }),
      switchMap(() => this._http.authSignUp(data)),
      tap((resp) => {
        if (resp.isSuccess && resp.data?.tokenData?.token != null) {
          this._authRepo.updateUserToken(
            resp.data?.user ?? null,
            resp.data?.tokenData ?? null
          );
          this._router.navigate(['private', 'profile']).then((res) => {
            this._authRepo.setLoading(false);
          });
        } else {
          let message = 'Произошла ошибка';
          if (resp.errors != null && resp.errors[0] != null) {
            message = resp.errors[0].message;
          }
          this._snackBarService.showError(message, 'Ошибка');
        }
        this._authRepo.setLoading(false);
      }),
      catchError((err) => {
        this._authRepo.setLoading(false);
        this._snackBarService.showError(err.Message, 'Ошибка');
        return EMPTY;
      }),
      mapTo(void 0)
    );
  }

  checkUserLoggedIn(): Observable<boolean> {
    return combineLatest([
      this._authRepo.token$
    ]).pipe(
      take(1),
      map(([token]) => {
        if (token == null || token.token == null) {
          return false;
        }
        const expireDate = new Date(token.expiryDate ?? '');
        const currentDate = new Date();
        if (expireDate < currentDate) {
          return false;
        }
        return true;
      })
    );
  }

  signOut(): Observable<void> {
    return of(0).pipe(
      tap(() => {
        this._authRepo.setLoading(true);
      }),
      tap(() => {
        this._authRepo.updateUserToken(null, null);
        this._authRepo.setLoading(false);
        this._router.navigate(['public', 'auth']).then();
      }),
      catchError((err) => {
        this._authRepo.setLoading(false);
        return EMPTY;
      }),
      mapTo(void 0)
    );
  }
}
