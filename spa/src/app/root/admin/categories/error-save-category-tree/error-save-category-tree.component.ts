import {
  ChangeDetectionStrategy,
  Component,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import {
  BehaviorSubject,
  filter,
  Observable,
  Subject,
  switchMap,
  takeUntil,
  tap,
} from 'rxjs';
import {
  ICategoryItemData,
  ICategoryTreeData,
} from 'src/app/shared/services/dto-models/category/category-tree-data';
import { CategoryService } from '../category.service';

@Component({
  selector: 'str-error-save-category-tree',
  template: '',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ErrorSaveCategoryTreeComponent implements OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  constructor(
    private _categoryService: CategoryService
  ) {}

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }


}
