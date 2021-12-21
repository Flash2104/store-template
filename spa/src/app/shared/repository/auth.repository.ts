import { Injectable } from '@angular/core';
import { createState, select, Store, withProps } from '@ngneat/elf';
import { localStorageStrategy, persistState } from '@ngneat/elf-persist-state';
import { Observable } from 'rxjs';
import { ITokenData } from '../services/dto-models/auth/token-data';
import { IUserData } from '../services/dto-models/auth/user-data';

export interface IAuthState {
  user: IUserData | null;
  token: ITokenData | null;
  loading: boolean;
}

const { state, config } = createState(
  withProps<IAuthState>({ user: null, token: null, loading: false })
);

const name = 'auth';

const authStore = new Store({ state, name, config });

export const authPersist = persistState(authStore, {
  source: (st) => st.pipe(select((s) => ({ token: s.token }))),
  key: name,
  storage: localStorageStrategy,
});

@Injectable({ providedIn: 'root' })
export class AuthRepository {
  user$: Observable<IUserData | null> = authStore.pipe(
    select((state) => state.user)
  );

  token$: Observable<ITokenData | null> = authStore.pipe(
    select((state) => state.token)
  );

  loading$: Observable<boolean> = authStore.pipe(
    select((state) => state.loading)
  );

  setLoading(loading: IAuthState['loading']): void {
    authStore.update((state) => ({
      ...state,
      loading,
    }));
  }

  updateUserToken(
    user: IAuthState['user'] | null,
    token: IAuthState['token']
  ): void {
    authStore.update((state) => ({
      ...state,
      user,
      token,
    }));
  }
}
