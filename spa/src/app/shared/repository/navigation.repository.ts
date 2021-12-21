import { Injectable } from '@angular/core';
import { createState, select, Store, withProps } from '@ngneat/elf';
import { Observable } from 'rxjs';
import { IUserNavigationData } from '../services/dto-models/navigations/navigation-data';

export interface INavigationState {
  data: IUserNavigationData[] | null | undefined;
  loading: boolean;
}

const { state, config } = createState(
  withProps<INavigationState>({
    data: null,
    loading: false,
  })
);

const name = 'navigation';

const profileStore = new Store({ state, name, config });

@Injectable({ providedIn: 'root' })
export class NavigationRepository {
  navData$: Observable<IUserNavigationData[] | null | undefined> =
    profileStore.pipe(select((st) => st.data));

  loading$: Observable<boolean> = profileStore.pipe(select((st) => st.loading));

  setLoading(loading: INavigationState['loading']): void {
    profileStore.update((state) => ({
      ...state,
      loading,
    }));
  }

  setData(data: INavigationState['data']): void {
    profileStore.update((st) => ({
      ...st,
      data,
    }));
  }

  destroy(): void {
    profileStore.complete();
    profileStore.destroy();
  }
}
