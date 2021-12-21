import { IRegionData } from './../services/dto-models/references/cities/cities-dto';
import { Injectable, OnDestroy } from '@angular/core';
import { createState, select, Store, withProps } from '@ngneat/elf';
import { selectAll, upsertEntities, withEntities } from '@ngneat/elf-entities';
import { Observable } from 'rxjs';
import { v1 as uuidv1 } from 'uuid';

@Injectable()
export class CitiesRepository implements OnDestroy {
  _state: {
    state: { entities: Record<string, IRegionData> };
    config: { idKey: 'id' };
  } = createState(
    withEntities<IRegionData>({ initialValue: [], idKey: 'id' }),
    withProps<{ loading: boolean }>({ loading: false })
  );

  _name: string = `cities-byRegions-${uuidv1().substring(0, 8)}`;

  citiesStore: Store<{
    state: {
      entities: Record<string, IRegionData>;
      ids: string[];
      loading: boolean;
    };
    name: string;
    config: undefined;
  }> = new Store({
    state: this._state.state,
    name: this._name,
    config: this._state.config,
  });

  cities$: Observable<IRegionData[] | null> = this.citiesStore.pipe(
    selectAll()
  );

  loading$: Observable<boolean> = this.citiesStore.pipe(
    select((st) => st.loading)
  );

  setLoading(loading: boolean): void {
    this.citiesStore.update((state) => ({
      ...state,
      loading,
    }));
  }

  upsertCities(data: IRegionData[] | null): void {
    if (data != null) {
      this.citiesStore.update(upsertEntities(data));
    }
  }

  ngOnDestroy(): void {
    this.citiesStore.complete();
    this.citiesStore.destroy();
  }
}
