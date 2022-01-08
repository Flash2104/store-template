import { Injectable, OnDestroy } from '@angular/core';
import { createState, select, Store, withProps } from '@ngneat/elf';
import { selectAll, upsertEntities, withEntities } from '@ngneat/elf-entities';
import { Observable } from 'rxjs';
import { ICategoryTreeData } from 'src/app/shared/services/dto-models/category/category-tree-data';
import { v1 as uuidv1 } from 'uuid';

@Injectable()
export class CategoryRepository implements OnDestroy {
  _state: {
    state: { entities: Record<number, ICategoryTreeData> };
    config: { idKey: 'id' };
  } = createState(
    withEntities<ICategoryTreeData>({ initialValue: [], idKey: 'id' }),
    withProps<{ loading: boolean }>({ loading: false })
  );

  _name: string = `categories-${uuidv1().substring(0, 8)}`;

  _store: Store<{
    state: {
      entities: Record<number, ICategoryTreeData>;
      ids: number[];
      loading: boolean;
    };
    name: string;
    config: { idKey: 'id' };
  }> = new Store({
    state: this._state.state,
    name: this._name,
    config: this._state.config,
  });

  categories$: Observable<ICategoryTreeData[] | null> = this._store.pipe(
    selectAll()
  );

  loading$: Observable<boolean> = this._store.pipe(
    select((st) => st.loading)
  );

  setLoading(loading: boolean): void {
    this._store.update((state) => ({
      ...state,
      loading,
    }));
  }

  upsertCategories(data: ICategoryTreeData[] | null): void {
    if (data != null) {
      this._store.update(upsertEntities(data));
    }
  }

  ngOnDestroy(): void {
    this._store.complete();
    this._store.destroy();
  }
}
