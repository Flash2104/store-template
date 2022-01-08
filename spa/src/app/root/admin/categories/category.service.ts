import { Injectable } from '@angular/core';
import { catchError, EMPTY, mapTo, Observable, of, switchMap, tap } from 'rxjs';
import { HttpService } from 'src/app/shared/services/http.service';
import { SnackbarService } from 'src/app/shared/services/snackbar.service';
import { CategoryRepository } from './category.repository';

@Injectable()
export class CategoryService {
  constructor(
    private _http: HttpService,
    private _categoryRepo: CategoryRepository,
    private _snackBarService: SnackbarService
  ) {}

  loadCategories(): Observable<void> {
    return of(0).pipe(
      tap(() => {
        this._categoryRepo.setLoading(true);
      }),
      switchMap(() => this._http.listCategoryTrees()),
      tap((resp) => {
        if (resp.isSuccess && resp.data != null) {
          this._categoryRepo.upsertCategories(resp.data.trees ?? null);
        } else {
          let message = 'Произошла ошибка';
          if (resp.errors != null && resp.errors[0] != null) {
            message = resp.errors[0].message;
          }
          this._snackBarService.showError(message, 'Ошибка');
        }
        this._categoryRepo.setLoading(false);
      }),
      catchError((err) => {
        this._categoryRepo.setLoading(false);
        this._snackBarService.showError(err.Message, 'Ошибка');
        return EMPTY;
      }),
      mapTo(void 0)
    );
  }
}
