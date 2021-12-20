import { Injectable, OnDestroy } from '@angular/core';
import { createState, select, Store, withProps } from '@ngneat/elf';
import { map, Observable } from 'rxjs';
import { v1 as uuidv1 } from 'uuid';
import {
  IMemberViewData,
  ITeamData,
} from '../../../shared/services/dto-models/team/team-data';

export interface ITeamMemberMainInfo {
  id: string | null | undefined;
  name: string | null | undefined;
  surname: string | null | undefined;
  city: string | null | undefined;
  isLeader: boolean | null | undefined;
}

export interface ITeamMainInfo {
  id: string | null | undefined;
  teamLeader: ITeamMemberMainInfo | null | undefined;
  city: string | null | undefined;
  title: string | null | undefined;
  foundationDate: string | null | undefined;
}

export interface ITeamState {
  team: ITeamData | null;
  noTeam: boolean;
  teamMainInfo: {
    data: ITeamMainInfo | null;
    isEditing: boolean;
    loading: boolean;
  };
  loading: boolean;
}

@Injectable()
export class TeamRepository implements OnDestroy {
  _state: {
    state: ITeamState;
    config: undefined;
  } = createState(
    withProps<ITeamState>({
      team: null,
      loading: false,
      noTeam: true,
      teamMainInfo: {
        data: null,
        isEditing: false,
        loading: false,
      },
    })
  );

  _name: string = `team-${uuidv1().substring(0, 8)}`;

  teamStore: Store<
    { state: ITeamState; name: string; config: undefined },
    ITeamState
  > = new Store({
    state: this._state.state,
    name: this._name,
    config: this._state.config,
  });

  team$: Observable<ITeamData | null> = this.teamStore.pipe(
    select((st) => st.team)
  );

  isEditing$: Observable<boolean> = this.teamStore.pipe(
    select((st) => st.teamMainInfo.isEditing)
  );

  isNoTeam$: Observable<boolean> = this.teamStore.pipe(
    select((st) => st.noTeam)
  );

  teamLeader$: Observable<IMemberViewData | null | undefined> =
    this.teamStore.pipe(
      select((st) => st.team?.members),
      map((v) => {
        return v?.find((m) => m.isLeader);
      })
    );

  loading$: Observable<boolean> = this.teamStore.pipe(
    select((st) => st.loading)
  );

  setLoading(loading: ITeamState['loading']): void {
    this.teamStore.update((state) => ({
      ...state,
      loading,
    }));
  }

  setTeam(team: ITeamState['team']): void {
    this.teamStore.update((st) => ({
      ...st,
      team,
      noTeam: false,
    }));
  }

  editingTeamMainInfo(isEditing: boolean): void {
    this.teamStore.update((st) => ({
      ...st,
      teamMainInfo: {
        ...st.teamMainInfo,
        isEditing,
        loading: st.teamMainInfo.loading,
      },
    }));
  }

  destroy(): void {
    this.teamStore.complete();
    this.teamStore.destroy();
  }

  ngOnDestroy(): void {
    this.destroy();
  }
}
