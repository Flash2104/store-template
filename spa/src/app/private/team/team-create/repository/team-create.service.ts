import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import {
  catchError,
  EMPTY,
  map,
  mapTo,
  Observable,
  of,
  switchMap,
  tap,
  withLatestFrom,
} from 'rxjs';
import { HttpService } from 'src/app/shared/services/http.service';
import { SnackbarService } from 'src/app/shared/services/snackbar.service';
import { ICreateTeamRequest } from '../../../../shared/services/dto-models/team/create/create-team-dto';
import { TeamCreateRepository } from './team-create.repository';

@Injectable()
export class TeamCreateService {
  constructor(
    private _http: HttpService,
    private _teamCreateRepo: TeamCreateRepository,
    private _snackBarService: SnackbarService,
    private _router: Router
  ) {}

  createTeam(): Observable<void> {
    return of(0).pipe(
      tap(() => {
        this._teamCreateRepo.setLoading(true);
      }),
      withLatestFrom(this._teamCreateRepo.createData$),
      tap(([, data]) => {
        if (data == null) {
          throw new Error('Пустой объект для сохранения');
        }
      }),
      map(([, data]): ICreateTeamRequest => {
        return {
          title: data!.title!,
          cityId: data?.city?.id,
          avatar: null,
          foundationDate: data?.foundationDate,
        };
      }),

      switchMap((data) => this._http.teamCreate(data)),
      tap((resp) => {
        if (!resp.isSuccess || resp.data == null) {
          let message = 'Произошла ошибка';
          if (resp.errors != null && resp.errors[0] != null) {
            message = resp.errors[0].message;
          }
          this._snackBarService.showError(message, 'Ошибка');
        } else {
          this._router.navigate(['private', 'team']).then(() => {
            this._teamCreateRepo.setLoading(false);
          });
        }
        this._teamCreateRepo.setLoading(false);
      }),
      catchError((err) => {
        this._teamCreateRepo.setLoading(false);
        this._snackBarService.showError(err.Message, 'Ошибка');
        return EMPTY;
      }),
      mapTo(void 0)
    );
  }
}
