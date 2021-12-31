import { Injectable } from '@angular/core';
import { createState, select, Store, withProps } from '@ngneat/elf';
import { Observable } from 'rxjs';

export interface IShopData {
  title: string;
  logo: string | null | undefined;
}

export interface IShopState {
  shop: IShopData | null;
  loading: boolean;
}

const { state, config } = createState(
  withProps<IShopState>({
    shop: null,
    loading: true,
  })
);

const name = 'shop';

const shopStore = new Store({ state, name, config });

@Injectable({ providedIn: 'root' })
export class ShopRepository {
  shop$: Observable<IShopData | null> = shopStore.pipe(select((st) => st.shop));

  loading$: Observable<boolean> = shopStore.pipe(select((st) => st.loading));

  setLoading(loading: IShopState['loading']): void {
    shopStore.update((state) => ({
      ...state,
      loading,
    }));
  }

  setShopData(shop: IShopState['shop']): void {
    shopStore.update((st) => ({
      ...st,
      shop
    }));
  }

  destroy(): void {
    shopStore.complete();
    shopStore.destroy();
  }
}
