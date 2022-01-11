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
import { IItemNode } from '../../../shared/components/editable-tree/editable-tree.component';
import {
  CategoryRepository,
  ICategoryTreeEditData,
} from './category.repository';
import { CategoryService } from './category.service';

@Component({
  selector: 'str-admin-category-trees',
  templateUrl: './admin-category-trees.component.html',
  styleUrls: ['./admin-category-trees.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [CategoryService, CategoryRepository],
})
export class AdminCategoryTreesComponent implements OnInit, OnDestroy {
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
          this.treeItems$.next(
            v.items != null ? this.mapToTreeItems(v.items, null) : []
          );
        }
      }),
      takeUntil(this._destroy$)
    );

  treeItems$: BehaviorSubject<IItemNode[]> = new BehaviorSubject<IItemNode[]>(
    []
  );

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
    this.treeItems$.complete();
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

  // hasChild(a: number, node: ICategoryItemData): boolean {
  //   return !!node.children && node.children.length > 0;
  // }

  mapToTreeItems(
    items: ICategoryItemData[],
    parent: IItemNode | null
  ): IItemNode[] {
    let isRoot = false;
    if (parent == null) {
      parent = {
        title: 'root',
        children: [],
        order: 0,
        parent: null,
      };
      isRoot = true;
    }
    const result = items
      .sort((a, b) => a.order - b.order)
      .map((element) => {
        const node = {
          id: element.id,
          title: element.title,
          order: element.order,
        } as IItemNode;
        node.children =
          (element.children && this.mapToTreeItems(element.children, node)) ||
          [];
        node.parent = parent;
        return node;
      });
    if (isRoot) {
      parent.children = result;
    }
    return result;
  }

  findTreeById(
    trees: ICategoryTreeData[] | null
  ): ICategoryTreeData | null | undefined {
    return trees?.find((x) => x.id === this.form.controls.treeSelect.value);
  }
}
