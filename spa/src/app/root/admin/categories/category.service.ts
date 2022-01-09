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

  loadCategoryTrees(): Observable<void> {
    return of(0).pipe(
      tap(() => {
        this._categoryRepo.setLoading(true);
      }),
      switchMap(() => this._http.listCategoryTrees()),
      tap((resp) => {
        this._categoryRepo.upsertCategories(resp.data?.trees ?? null);
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

  loadCategoryTree(id: number): Observable<void> {
    return of(0).pipe(
      tap(() => {
        this._categoryRepo.setGetTreeLoading(true);
      }),
      switchMap(() => this._http.getCategoryTree(id)),
      tap((resp) => {
        this._categoryRepo.setSelectedTree(resp.data?.tree ?? null);
        this._categoryRepo.setGetTreeLoading(false);
      }),
      catchError((err) => {
        this._categoryRepo.setGetTreeLoading(false);
        this._snackBarService.showError(err.Message, 'Ошибка');
        return EMPTY;
      }),
      mapTo(void 0)
    );
  }
}
