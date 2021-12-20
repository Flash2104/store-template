import {
  ChangeDetectionStrategy,
  Component,
  Input,
  OnDestroy,
} from '@angular/core';
import { Subject } from 'rxjs';
import { IReferenceData } from '../../../../shared/services/dto-models/reference-data';
import { IMemberViewData } from '../../../../shared/services/dto-models/team/team-data';
import { TeamRepository } from '../../repository/team.repository';

@Component({
  selector: 'air-team-members',
  templateUrl: './team-members.component.html',
  styleUrls: ['./team-members.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TeamMembersComponent implements OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  @Input() teamMembers: IMemberViewData[] | null | undefined = [];
  @Input() loading: boolean | null = null;

  constructor(private _teamRepo: TeamRepository) {}

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }

  onEdit(): void {
    this._teamRepo.editingTeamMainInfo(true);
  }

  getRolesString(roles: IReferenceData<string>[] | null | undefined): string {
    return roles?.map((x) => x.title)?.join(', ') || '';
  }

  // openMemberInNew(id: string): void {

  // }
}
