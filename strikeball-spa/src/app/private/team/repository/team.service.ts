import { Injectable } from '@angular/core';
import { catchError, EMPTY, mapTo, Observable, of, switchMap, tap } from 'rxjs';
import { HttpService } from 'src/app/shared/services/http.service';
import { SnackbarService } from 'src/app/shared/services/snackbar.service';
import { TeamRepository } from './team.repository';

@Injectable()
export class TeamService {
  constructor(
    private _http: HttpService,
    private _teamRepo: TeamRepository,
    private _snackBarService: SnackbarService
  ) {}

  loadCurrentTeam(): Observable<void> {
    return of(0).pipe(
      tap(() => {
        this._teamRepo.setLoading(true);
      }),
      switchMap(() => this._http.teamGetCurrent()),
      tap((resp) => {
        if (resp.isSuccess && resp.data != null) {
          this._teamRepo.setTeam(resp.data?.teamData ?? null);
        } else {
          let message = 'Произошла ошибка';
          if (resp.errors != null && resp.errors[0] != null) {
            message = resp.errors[0].message;
          }
          this._snackBarService.showError(message, 'Ошибка');
        }
        this._teamRepo.setLoading(false);
      }),
      catchError((err) => {
        this._teamRepo.setLoading(false);
        this._snackBarService.showError(err.Message, 'Ошибка');
        return EMPTY;
      }),
      mapTo(void 0)
    );
  }
}
