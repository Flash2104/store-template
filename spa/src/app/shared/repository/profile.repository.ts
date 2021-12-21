import { Injectable } from '@angular/core';
import { createState, select, Store, withProps } from '@ngneat/elf';
import { Observable } from 'rxjs';
import { IProfileData } from '../services/dto-models/profile/profile-data';

export interface IProfileState {
  profile: IProfileData | null;
  loading: boolean;
}

const { state, config } = createState(
  withProps<IProfileState>({
    profile: null,
    loading: false,
  })
);

const name = 'profile';

const profileStore = new Store({ state, name, config });

@Injectable({ providedIn: 'root' })
export class ProfileRepository {
  profile$: Observable<IProfileData | null> = profileStore.pipe(
    select((st) => st.profile)
  );

  loading$: Observable<boolean> = profileStore.pipe(select((st) => st.loading));

  setLoading(loading: IProfileState['loading']): void {
    profileStore.update((state) => ({
      ...state,
      loading,
    }));
  }

  setProfile(profile: IProfileState['profile']): void {
    profileStore.update((st) => ({
      ...st,
      profile,
    }));
  }

  destroy(): void {
    profileStore.complete();
    profileStore.destroy();
  }
}
