import {
  ChangeDetectionStrategy,
  Component,
  Input,
  OnDestroy,
} from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Subject } from 'rxjs';
import {
  ITeamMainInfo,
  TeamRepository,
} from '../../repository/team.repository';

@Component({
  selector: 'air-team-main-info-edit',
  templateUrl: './team-main-info-edit.component.html',
  styleUrls: ['./team-main-info-edit.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TeamMainInfoEditComponent implements OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  @Input() team: ITeamMainInfo | null = null;
  @Input() loading: boolean | null = null;

  form: FormGroup = new FormGroup({});

  constructor(private _teamRepo: TeamRepository) {}

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }

  onCancel(): void {
    this._teamRepo.editingTeamMainInfo(false);
  }

  onSave(): void {
    this._teamRepo.editingTeamMainInfo(true);
  }
}
