import { Injectable } from '@angular/core';
import { createState, select, Store, withProps } from '@ngneat/elf';
import { Observable } from 'rxjs';

export interface IShopData {
  title: string;
  logo?: string | null | undefined;
}

export interface IShopState {
  origin: IShopData;
  changed: IShopData;
  isChanged: boolean;
  loading: boolean;
}

const { state, config } = createState(
  withProps<IShopState>({
    origin: {
      title: ''
    },
    changed: {
      title: ''
    },
    isChanged: false,
    loading: true,
  })
);

const name = 'shop';

const shopStore = new Store({ state, name, config });

@Injectable({ providedIn: 'root' })
export class ShopRepository {
  origin$: Observable<IShopData> = shopStore.pipe(select((st) => st.origin));
  changed$: Observable<IShopData> = shopStore.pipe(select((st) => st.changed));
  isChanged$: Observable<boolean> = shopStore.pipe(select((st) => st.isChanged));
  loading$: Observable<boolean> = shopStore.pipe(select((st) => st.loading));

  setLoading(loading: IShopState['loading']): void {
    shopStore.update((state) => ({
      ...state,
      loading,
    }));
  }

  onLoadShopData(shop: IShopState['origin']): void {
    shopStore.update((st) => ({
      ...st,
      origin: shop,
      changed: shop,
      isChanged: false
    }));
  }

  changeTitle(title: string): void {
    shopStore.update((st) => ({
      ...st,
      changed: {
        ...st.changed,
        title
      },
      isChanged: true
    }));
  }

  changeLogo(logo: string | null | undefined): void {
    shopStore.update((st) => ({
      ...st,
      changed: {
        title: st.changed.title,
        logo
      }
    }));
  }

  resetChanged(): void {
    shopStore.update((st) => ({
      ...st,
      changed: st.origin,
      isChanged: false
    }));
  }

  destroy(): void {
    shopStore.complete();
    shopStore.destroy();
  }
}
