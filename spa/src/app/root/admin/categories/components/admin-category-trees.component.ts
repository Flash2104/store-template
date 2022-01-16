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
import { IItemNode } from 'src/app/shared/components/editable-tree/editable-tree.component';
import {
  ICategoryItemData,
  ICategoryTreeData,
} from 'src/app/shared/services/dto-models/category/category-tree-data';
import { CategoryRepository, ICategoryTreeEditData } from '../repository/category.repository';
import { CategoryService } from '../repository/category.service';

@Component({
  selector: 'str-admin-category-trees',
  templateUrl: './admin-category-trees.component.html',
  styleUrls: ['./admin-category-trees.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AdminCategoryTreesComponent implements OnInit, OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  loading$: Observable<boolean> = this._categoryRepo.loading$.pipe(
    takeUntil(this._destroy$)
  );

  loadingTree$: Observable<boolean> = this._categoryRepo.loadingTree$.pipe(
    takeUntil(this._destroy$)
  );

  updateTreeLoading$: Observable<boolean> = this._categoryRepo.updateTreeLoading$.pipe(
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
          this.root$.next(v.root);
        } else {
          this.root$.next(null);
        }
      }),
      takeUntil(this._destroy$)
    );

  root$: BehaviorSubject<IItemNode | null | undefined> =
    new BehaviorSubject<IItemNode | null | undefined>(null);

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
    this.root$.complete();
  }

  onOriginal(): void {
    this._categoryRepo.resetChanged();
  }

  createTree(): void {
    this._categoryRepo.createNewTree();
  }

  onTreeChanged(editTree: ICategoryTreeEditData, editRoot: IItemNode): void {
    this._categoryRepo.updateEditTree(editRoot);
    if(editTree.id !== 0) {
      this._categoryService.updateCategoryTree().subscribe();
    }
  }

  onSave(): void {
    // this._shopService.updateShopInfo().subscribe();
  }

  mapToTreeItems(
    items: ICategoryItemData[] | null,
    parent: IItemNode | null
  ): IItemNode[] {
    const result = items
      ?.sort((a, b) => a.order - b.order)
      ?.map((element) => {
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
    return result || [];
  }

  findTreeById(
    trees: ICategoryTreeData[] | null
  ): ICategoryTreeData | null | undefined {
    return trees?.find((x) => x.id === this.form.controls.treeSelect.value);
  }
}
