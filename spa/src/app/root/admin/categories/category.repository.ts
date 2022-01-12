import { Injectable, OnDestroy } from '@angular/core';
import { createState, select, Store, withProps } from '@ngneat/elf';
import { selectAll, upsertEntities, withEntities } from '@ngneat/elf-entities';
import { Observable } from 'rxjs';
import {
  ICategoryItemData,
  ICategoryTreeData,
} from 'src/app/shared/services/dto-models/category/category-tree-data';
import { v1 as uuidv1 } from 'uuid';

export interface ICategoryTreeEditData {
  id?: number | null | undefined;
  title?: string | null | undefined;
  isDefault?: boolean | null | undefined;
  items?: ICategoryItemData[] | null | undefined;
}

@Injectable()
export class CategoryRepository implements OnDestroy {
  _state: {
    state: { entities: Record<number, ICategoryTreeData> };
    config: { idKey: 'id' };
  } = createState(
    withEntities<ICategoryTreeData>({ initialValue: [], idKey: 'id' }),
    withProps<{
      loading: boolean;
      loadingTree: boolean;
      isChanged: boolean;
      editTree: ICategoryTreeEditData | null;
      originalTree: ICategoryTreeEditData | null;
    }>({
      loading: false,
      loadingTree: false,
      isChanged: false,
      editTree: null,
      originalTree: null,
    })
  );

  _name: string = `categories-${uuidv1().substring(0, 8)}`;

  _store: Store<{
    state: {
      entities: Record<number, ICategoryTreeData>;
      ids: number[];
      loadingTree: boolean;
      loading: boolean;
      isChanged: boolean;
      editTree: ICategoryTreeEditData | null;
      originalTree: ICategoryTreeEditData | null;
    };
    name: string;
    config: { idKey: 'id' };
  }> = new Store({
    state: this._state.state,
    name: this._name,
    config: this._state.config,
  });

  trees$: Observable<ICategoryTreeData[] | null> = this._store.pipe(
    selectAll()
  );

  loading$: Observable<boolean> = this._store.pipe(select((st) => st.loading));
  loadingTree$: Observable<boolean> = this._store.pipe(
    select((st) => st.loadingTree)
  );

  isChanged$: Observable<boolean> = this._store.pipe(
    select((st) => st.isChanged)
  );

  editTree$: Observable<ICategoryTreeEditData | null> = this._store.pipe(
    select((st) => st.editTree)
  );

  originalTree$: Observable<ICategoryTreeEditData | null> = this._store.pipe(
    select((st) => st.originalTree)
  );

  setLoading(loading: boolean): void {
    this._store.update((state) => ({
      ...state,
      loading,
    }));
  }

  setGetTreeLoading(loading: boolean): void {
    this._store.update((state) => ({
      ...state,
      loadingTree: loading,
    }));
  }

  upsertCategories(data: ICategoryTreeData[] | null): void {
    if (data != null) {
      this._store.update(upsertEntities(data));
    }
  }

  setIsDefault(value: boolean): void {
    this._store.update((st) => ({
      ...st,
      editTree: { ...st.editTree, isDefault: value },
      isChanged: true,
    }));
  }

  setTitle(value: string): void {
    this._store.update((st) => ({
      ...st,
      editTree: { ...st.editTree, title: value },
      isChanged: true,
    }));
  }

  setSelectedTree(data: ICategoryTreeData | null): void {
    this._store.update((st) => ({
      ...st,
      editTree: data,
      originalTree: data,
      isChanged: false,
    }));
  }

  setSelectedCategory(data: ICategoryItemData | null): void {
    this._store.update((st) => ({
      ...st,
      editCategory: data,
    }));
  }

  createNewTree(): void {
    this._store.update((st) => ({
      ...st,
      editTree: {
        id: 0,
        isDefault: false,
        title: 'Новое дерево категорий',
        items: [],
      },
      originalTree: null,
      isChanged: true,
    }));
  }

  resetChanged(): void {
    this._store.update((st) => ({
      ...st,
      editTree: st.originalTree != null ? { ...st.originalTree } : null,
      isChanged: false,
    }));
  }

  resetSelectedCategory(): void {
    this._store.update((st) => ({
      ...st,
      editCategory: null,
    }));
  }

  ngOnDestroy(): void {
    this._store.complete();
    this._store.destroy();
  }
}
