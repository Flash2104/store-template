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
import { ICategoryItemData } from 'src/app/shared/services/dto-models/category/category-tree-data';

@Component({
  selector: 'str-admin-categories',
  templateUrl: './admin-categories.component.html',
  styleUrls: ['./admin-categories.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [CategoryService, CategoryRepository]
})
export class AdminCategoriesComponent implements OnInit, OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  isChanged$: Observable<boolean> = this._categoryRepo.isChanged$.pipe(
    takeUntil(this._destroy$)
  );

  treeControl: NestedTreeControl<ICategoryItemData> = new NestedTreeControl<ICategoryItemData>(
    (node) => node.children
  );

  dataSource: MatTreeNestedDataSource<ICategoryItemData> = new MatTreeNestedDataSource<ICategoryItemData>();

  constructor(
    private _categoryService: CategoryService,
    private _categoryRepo: CategoryRepository
  ) {}

  ngOnInit(): void {
      this._categoryService.loadCategories().subscribe();
      this._categoryRepo.trees$
        .pipe(
          tap((tree) => {
            if (tree != null) {
              const defaultData = tree.find((x) => x.isDefault)?.items;
              const data = defaultData != null ? this.sortItems(defaultData) : [];
              this.dataSource.data = [...data];
            }
          }),
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
    // this._categoryRepo.resetChanged();
  }

  onLogoChange(): void {
    console.log('Logo changed');
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
