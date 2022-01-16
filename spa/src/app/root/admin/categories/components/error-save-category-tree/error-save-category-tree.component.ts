import {
  ChangeDetectionStrategy,
  Component,
  Inject,
  OnDestroy,
} from '@angular/core';
import {
  MatSnackBarRef,
  MAT_SNACK_BAR_DATA,
} from '@angular/material/snack-bar';
import { Subject } from 'rxjs';
import { CategoryService } from '../../repository/category.service';

export interface ISaveCategoryTreeError {
  message: string | null;
}

@Component({
  selector: 'str-error-save-category-tree',
  styleUrls: ['./error-save-category-tree.component.scss'],
  templateUrl: './error-save-category-tree.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ErrorSaveCategoryTreeComponent implements OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  constructor(
    @Inject(MAT_SNACK_BAR_DATA) public data: ISaveCategoryTreeError,
    private _snackRef: MatSnackBarRef<ErrorSaveCategoryTreeComponent>,
    private _categoryService: CategoryService
  ) {}

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }

  retry(): void {
    this._snackRef.dismiss();
    this._categoryService.updateCategoryTree().subscribe();
  }

  close(): void {
    this._snackRef.dismiss();
  }
}
