import { Injectable, OnDestroy } from '@angular/core';
import { createState, select, Store, withProps } from '@ngneat/elf';
import { selectAll, upsertEntities, withEntities } from '@ngneat/elf-entities';
import { Observable } from 'rxjs';
import {
  IChangedEventData,
  IItemNode,
} from 'src/app/shared/components/editable-tree/editable-tree.component';
import {
  ICategoryItemData,
  ICategoryTreeData,
} from 'src/app/shared/services/dto-models/category/category-tree-data';
import { v1 as uuidv1 } from 'uuid';

export interface ICategoryTreeEditData {
  id?: number | null | undefined;
  title?: string | null | undefined;
  isDefault?: boolean | null | undefined;
  root?: IItemNode | null | undefined;
  removedIds?: (string | number)[] | null | undefined;
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
      updateTreeLoading: boolean;
      editTree: ICategoryTreeEditData | null;
      originalTree: ICategoryTreeData | null;
    }>({
      loading: false,
      loadingTree: false,
      updateTreeLoading: false,
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
      updateTreeLoading: boolean;
      loading: boolean;
      editTree: ICategoryTreeEditData | null;
      originalTree: ICategoryTreeData | null;
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

  updateTreeLoading$: Observable<boolean> = this._store.pipe(
    select((st) => st.updateTreeLoading)
  );

  editTree$: Observable<ICategoryTreeEditData | null> = this._store.pipe(
    select((st) => st.editTree)
  );

  originalTree$: Observable<ICategoryTreeData | null> = this._store.pipe(
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

  setUpdateTreeLoading(loading: boolean): void {
    this._store.update((state) => ({
      ...state,
      updateTreeLoading: loading,
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
    }));
  }

  setTitle(value: string): void {
    this._store.update((st) => ({
      ...st,
      editTree: { ...st.editTree, title: value },
    }));
  }

  setSelectedTree(data: ICategoryTreeData | null): void {
    this._store.update((st) => ({
      ...st,
      editTree: {
        id: data?.id,
        isDefault: data?.isDefault,
        root: this._createRoot(data),
        title: data?.title,
      },
      originalTree: data,
    }));
  }

  updateOriginalTree(data: ICategoryTreeData): void {
    this._store.update((st) => ({
      ...st,
      originalTree: { ...data },
    }));
  }

  updateEditTreeItems(data: ICategoryTreeData): void {
    this._store.update((st) => {
      const editTree = st.editTree;
      if (editTree != null) {
        editTree.removedIds = null;
        if (editTree.root != null) {
          editTree.root.children =
            data?.items != null
              ? this.mapToTreeItems(data.items, editTree.root)
              : [];
        }
      }
      return {
        ...st,
        editTree,
      };
    });
  }

  updateEditTree(changedData: IChangedEventData): void {
    this._store.update((st) => ({
      ...st,
      editTree: {
        ...st.editTree,
        root: changedData.updatedRoot,
        removedIds:
          changedData.removedIds != null && changedData.removedIds.length > 0
            ? [...changedData.removedIds]
            : [],
      },
    }));
  }

  createNewTree(): void {
    this._store.update((st) => ({
      ...st,
      editTree: {
        id: 0,
        isDefault: false,
        title: 'Новое дерево категорий',
        root: {
          title: 'root',
          children: [],
          order: 0,
          parent: null,
        },
      },
      originalTree: null,
    }));
  }

  resetChanged(): void {
    this._store.update((st) => ({
      ...st,
      editTree:
        st.originalTree != null
          ? {
              id: st.originalTree.id,
              isDefault: st.originalTree.isDefault,
              title: st.originalTree.title,
              root: this._createRoot(st.originalTree),
            }
          : null,
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

  // mapOldRefTreeItems(
  //   items: ICategoryItemData[] | null,
  //   parent: IItemNode | null
  // ): IItemNode[] {
  //   items?.forEach((element) => {
  //       const node = {
  //         id: element.id,
  //         title: element.title,
  //         order: element.order,
  //       } as IItemNode;
  //       node.children =
  //         (element.children && this.mapToTreeItems(element.children, node)) ||
  //         [];
  //       node.parent = parent;
  //       return node;
  //     });
  //   return items || [];
  // }

  private _createRoot(data: ICategoryTreeData | null): IItemNode {
    const root: IItemNode = {
      title: 'root',
      children: [],
      order: 0,
      parent: null,
    };
    root.children =
      data?.items != null ? this.mapToTreeItems(data.items, root) : [];
    return root;
  }
}
