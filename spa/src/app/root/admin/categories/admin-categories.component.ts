import { NestedTreeControl } from '@angular/cdk/tree';
import {
  ChangeDetectionStrategy,
  Component,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatTreeNestedDataSource } from '@angular/material/tree';
import { filter, Observable, Subject, switchMap, takeUntil, tap } from 'rxjs';
import {
  ICategoryItemData,
  ICategoryTreeData,
} from 'src/app/shared/services/dto-models/category/category-tree-data';
import {
  CategoryRepository,
  ICategoryTreeEditData,
} from './category.repository';
import { CategoryService } from './category.service';

@Component({
  selector: 'str-admin-categories',
  templateUrl: './admin-categories.component.html',
  styleUrls: ['./admin-categories.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [CategoryService, CategoryRepository],
})
export class AdminCategoriesComponent implements OnInit, OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();
  isChanged$: Observable<boolean> = this._categoryRepo.isChanged$.pipe(
    takeUntil(this._destroy$)
  );

  loading$: Observable<boolean> = this._categoryRepo.loading$.pipe(
    takeUntil(this._destroy$)
  );

  loadingTree$: Observable<boolean> = this._categoryRepo.loadingTree$.pipe(
    takeUntil(this._destroy$)
  );

  editTree$: Observable<ICategoryTreeEditData | null> =
    this._categoryRepo.editTree$.pipe(
      tap((v) => {
        if (v != null) {
          this.form.controls.title.setValue(v.title, { emitEvent: false });
          this.form.controls.isDefault.setValue(v.isDefault, {
            emitEvent: false,
          });
          const data = v.items != null ? this.sortItems(v.items) : [];
          this.dataSource.data = [...data];
        }
      }),
      takeUntil(this._destroy$)
    );

  dataSource: MatTreeNestedDataSource<ICategoryItemData> =
    new MatTreeNestedDataSource<ICategoryItemData>();

  treeControl: NestedTreeControl<ICategoryItemData> =
    new NestedTreeControl<ICategoryItemData>((node) => node.children);

  form: FormGroup = new FormGroup({
    title: new FormControl(null, [Validators.required]),
    isDefault: new FormControl(false),
    treeSelect: new FormControl(null),
  });

  trees$: Observable<ICategoryTreeData[] | null> =
    this._categoryRepo.trees$.pipe(takeUntil(this._destroy$));

  constructor(
    private _categoryService: CategoryService,
    private _categoryRepo: CategoryRepository
  ) {}

  ngOnInit(): void {
    this._categoryService.loadCategoryTrees().subscribe();
    this._categoryRepo.originalTree$
      .pipe(
        tap((v) => {
          this.form.controls.treeSelect.setValue(v?.id, {
            emitEvent: false,
          });
        }),
        takeUntil(this._destroy$)
      )
      .subscribe();
    this.form.controls.treeSelect.valueChanges
      .pipe(
        filter((v) => v != null),
        switchMap((v) => this._categoryService.loadCategoryTree(v)),
        takeUntil(this._destroy$)
      )
      .subscribe();

    this.form.controls.isDefault.valueChanges
      .pipe(
        tap((v) => this._categoryRepo.setIsDefault(v)),
        takeUntil(this._destroy$)
      )
      .subscribe();

    this.form.controls.title.valueChanges
      .pipe(
        tap((v) => this._categoryRepo.setTitle(v)),
        takeUntil(this._destroy$)
      )
      .subscribe();
  }

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
    this._categoryRepo.ngOnDestroy();
  }

  onCancel(): void {
    this._categoryRepo.resetChanged();
  }

  createTree(): void {
    this._categoryRepo.createNewTree();
  }

  onSave(): void {
    // this._shopService.updateShopInfo().subscribe();
  }

  hasChild(a: number, node: ICategoryItemData): boolean {
    return !!node.children && node.children.length > 0;
  }

  sortItems(items: ICategoryItemData[]): ICategoryItemData[] {
    items.forEach((element) => {
      if (element.children != null) {
        element.children = this.sortItems(element.children);
      }
    });
    return items.sort((a, b) => a.order - b.order);
  }

  findTreeById(
    trees: ICategoryTreeData[] | null
  ): ICategoryTreeData | null | undefined {
    return trees?.find((x) => x.id === this.form.controls.treeSelect.value);
  }
}
