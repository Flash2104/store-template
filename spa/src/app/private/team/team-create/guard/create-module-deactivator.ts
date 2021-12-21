import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanDeactivate,
  RouterStateSnapshot,
} from '@angular/router';
import { TeamCreateRepository } from './../repository/team-create.repository';

@Injectable()
export class CreateModuleDeactivator implements CanDeactivate<any> {
  public constructor(private _teamCreateRepo: TeamCreateRepository) {}

  public canDeactivate(
    component: any,
    currentRoute: ActivatedRouteSnapshot,
    currentState: RouterStateSnapshot,
    nextState?: RouterStateSnapshot
  ): boolean {
    this._teamCreateRepo.destroy();
    return true;
  }
}
