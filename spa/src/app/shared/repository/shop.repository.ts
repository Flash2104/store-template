import { Injectable } from '@angular/core';
import { createState, select, Store, withProps } from '@ngneat/elf';
import { Observable } from 'rxjs';

export interface IShopData {
  title?: string | null;
  logo?: string | null | undefined;
}

export interface IShopState {
  origin: IShopData | null;
  changed: IShopData | null;
  loading: boolean;
}

const { state, config } = createState(
  withProps<IShopState>({
    origin: null,
    changed: null,
    loading: true,
  })
);

const name = 'shop';

const shopStore = new Store({ state, name, config });

@Injectable({ providedIn: 'root' })
export class ShopRepository {
  origin$: Observable<IShopData | null> = shopStore.pipe(select((st) => st.origin));
  loading$: Observable<boolean> = shopStore.pipe(select((st) => st.loading));

  setLoading(loading: IShopState['loading']): void {
    shopStore.update((state) => ({
      ...state,
      loading,
    }));
  }

  setShopData(shop: IShopState['origin']): void {
    shopStore.update((st) => ({
      ...st,
      origin: shop
    }));
  }

  changeTitle(title: string): void {
    shopStore.update((st) => ({
      ...st,
      changed: {
        ...st.changed,
        title
      }
    }));
  }

  changeLogo(logo: string | null | undefined): void {
    shopStore.update((st) => ({
      ...st,
      changed: {
        ...st.changed,
        logo
      }
    }));
  }

  resetChanged(): void {
    shopStore.update((st) => ({
      ...st,
      changed: null
    }));
  }

  destroy(): void {
    shopStore.complete();
    shopStore.destroy();
  }
}
