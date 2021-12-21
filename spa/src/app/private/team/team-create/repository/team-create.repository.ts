import { Injectable, OnDestroy } from '@angular/core';
import { createState, select, Store, withProps } from '@ngneat/elf';
import { Observable } from 'rxjs';
import { IReferenceData } from 'src/app/shared/services/dto-models/reference-data';
import { v1 as uuidv1 } from 'uuid';

export interface ITeamCreateData {
  city?: IReferenceData<number> | null | undefined;
  title?: string | null | undefined;
  foundationDate?: string | null | undefined;
  avatar?: string | null | undefined;
}

export interface ITeamCreateState {
  createData: ITeamCreateData | null;
  loading: boolean;
}

@Injectable()
export class TeamCreateRepository implements OnDestroy {
  _state: {
    state: ITeamCreateState;
    config: undefined;
  } = createState(
    withProps<ITeamCreateState>({
      createData: null,
      loading: false,
    })
  );

  _name: string = `team-create-${uuidv1().substring(0, 8)}`;

  teamCreateStore: Store<
    { state: ITeamCreateState; name: string; config: undefined },
    ITeamCreateState
  > = new Store({
    state: this._state.state,
    name: this._name,
    config: this._state.config,
  });

  createData$: Observable<ITeamCreateData | null> = this.teamCreateStore.pipe(
    select((st) => st.createData)
  );

  loading$: Observable<boolean> = this.teamCreateStore.pipe(
    select((st) => st.loading)
  );

  setLoading(loading: ITeamCreateState['loading']): void {
    this.teamCreateStore.update((state) => ({
      ...state,
      loading,
    }));
  }

  patchCreateData(createData: Partial<ITeamCreateState['createData']>): void {
    this.teamCreateStore.update((st) => ({
      ...st,
      createData,
    }));
  }

  destroy(): void {
    this.teamCreateStore.complete();
    this.teamCreateStore.destroy();
  }

  ngOnDestroy(): void {
    this.destroy();
  }
}
