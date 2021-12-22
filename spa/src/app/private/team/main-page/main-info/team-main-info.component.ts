import {
  ChangeDetectionStrategy,
  Component,
  Input,
  OnDestroy,
} from '@angular/core';
import { Subject } from 'rxjs';
import {
  ITeamMainInfo,
  TeamRepository,
} from '../../repository/team.repository';

@Component({
  selector: 'str-team-main-info',
  templateUrl: './team-main-info.component.html',
  styleUrls: ['./team-main-info.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TeamMainInfoComponent implements OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  @Input() team: ITeamMainInfo | null = null;
  @Input() loading: boolean | null = null;

  constructor(private _teamRepo: TeamRepository) {}

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }

  onEdit(): void {
    this._teamRepo.editingTeamMainInfo(true);
  }
}
