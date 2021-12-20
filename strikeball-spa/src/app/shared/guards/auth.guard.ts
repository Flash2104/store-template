import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
} from '@angular/router';
import { combineLatest, Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';
import {
  authPersist,
  AuthRepository,
} from '../repository/auth.repository';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(
    private _authRepository: AuthRepository,
    private _router: Router
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> {
    return combineLatest([
      this._authRepository.token$,
      authPersist.initialized$,
    ]).pipe(
      take(1),
      map(([token]) => {
        if (token?.token != null) {
          this._router
            .navigate(
              ['private', 'profile']
            )
            .then();
          return false;
        }
        return true;
      })
    );
  }
}
