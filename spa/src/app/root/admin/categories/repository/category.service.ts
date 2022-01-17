import { Injectable } from '@angular/core';
import { catchError, EMPTY, map, mapTo, Observable, of, switchMap, tap, withLatestFrom, filter, throwError } from 'rxjs';
import { IItemNode } from 'src/app/shared/components/editable-tree/editable-tree.component';
import { ICategoryItemData } from 'src/app/shared/services/dto-models/category/category-tree-data';
import { ICreateCategoryTreeRequest } from 'src/app/shared/services/dto-models/category/create-category-tree';
import { HttpService } from 'src/app/shared/services/http.service';
import { SnackbarService } from 'src/app/shared/services/snackbar.service';
import { ErrorSaveCategoryTreeComponent, ISaveCategoryTreeError } from '../components/error-save-category-tree/error-save-category-tree.component';
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

  createCategoryTree(): Observable<void> {
    return of(0).pipe(
      tap(() => {
        this._categoryRepo.setUpdateTreeLoading(true);
      }),
      withLatestFrom(this._categoryRepo.editTree$),
      map(([_, et]): ICreateCategoryTreeRequest => {
        return {
          tree: {
            id: 0,
            title: et?.title || '',
            isDefault: et?.isDefault,
            items: et?.root?.children != null ? this._mapChildren(et?.root?.children) : []
          }
        }
      }),
      switchMap((data) => this._http.createCategoryTree(data)),
      tap((resp) => {
        this._categoryRepo.setSelectedTree(resp.data?.tree ?? null);
        this._categoryRepo.setUpdateTreeLoading(false);
      }),
      catchError((err) => {
        this._categoryRepo.setUpdateTreeLoading(false);
        this._snackBarService.showError(err.Message, 'Ошибка');
        return EMPTY;
      }),
      mapTo(void 0)
    );
  }

  updateCategoryTree(): Observable<void> {
    return of(0).pipe(
      tap(() => {
        this._categoryRepo.setUpdateTreeLoading(true);
      }),
      withLatestFrom(this._categoryRepo.editTree$),
      filter(([_, et]) => et != null && et.id != null && et.id !== 0),
      map(([_, et]): ICreateCategoryTreeRequest => {
        return {
          tree: {
            id: et?.id!,
            title: et?.title || '',
            isDefault: et?.isDefault,
            items: et?.root?.children != null ? this._mapChildren(et?.root?.children) : []
          }
        }
      }),
      switchMap((data) => this._http.updateCategoryTree(data)),
      tap((resp) => {
        if(resp.isSuccess && resp.data?.tree != null) {
          this._categoryRepo.updateOriginalTree(resp.data.tree);
        } else {
          const errorData: ISaveCategoryTreeError = {
            message: resp?.errors != null && resp?.errors?.length > 0 ? resp?.errors[0].message : null
          };
          this._snackBarService.openWithComponent(ErrorSaveCategoryTreeComponent, errorData);
        }
        this._categoryRepo.setUpdateTreeLoading(false);
      }),
      catchError((err) => {
        this._categoryRepo.setUpdateTreeLoading(false);
        this._snackBarService.showError(err.Message, 'Ошибка');
        return throwError(() => err);
      }),
      mapTo(void 0)
    );
  }

  private _mapChildren(els: IItemNode[]):ICategoryItemData[] {
    return els.map(x => ({
      id: x.id,
      title: x.title,
      icon: x.icon,
      isDisabled: x.isDisabled,
      order: x.order,
      children: this._mapChildren(x.children)
    } as ICategoryItemData));
  };
}
