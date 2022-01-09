import { FormGroup, FormControl, Validators } from '@angular/forms';
import { CategoryRepository } from './category.repository';
import { NestedTreeControl } from '@angular/cdk/tree';
import {
  ChangeDetectionStrategy,
  Component,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { MatTreeNestedDataSource } from '@angular/material/tree';
import { Observable, Subject, takeUntil, tap } from 'rxjs';
import { CategoryService } from './category.service';
import {
  ICategoryItemData,
  ICategoryTreeData,
} from 'src/app/shared/services/dto-models/category/category-tree-data';

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
    this._categoryRepo.trees$.pipe(
      tap((trees) => {
        if (trees != null && trees.length !== 0) {
          const defaultData = trees.find((x) => x.isDefault);
          if(defaultData != null) {
            const data = defaultData.items != null ? this.sortItems(defaultData.items) : [];
            this.dataSource.data = [...data];
            this.form.controls.treeSelect.setValue(defaultData.id);
          }
        }
      }),
      takeUntil(this._destroy$)
    );

  constructor(
    private _categoryService: CategoryService,
    private _categoryRepo: CategoryRepository
  ) {}

  ngOnInit(): void {
    this._categoryService.loadCategories().subscribe();
    this.form.controls.treeSelect.valueChanges.pipe(
      tap(v => console.log(v)),
      takeUntil(this._destroy$)
    ).subscribe();
  }

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
    this._categoryRepo.ngOnDestroy();
  }

  onCancel(): void {
    // this._categoryRepo.resetChanged();
  }

  createTree(): void {
    console.log('Новое дерево');
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
}
