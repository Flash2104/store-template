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
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root',
})
export class PrivateGuard implements CanActivate {
  constructor(
    private _authRepository: AuthRepository,
    private _authService: AuthService,
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
        const navigateAuth = (): void => {
          this._router.navigate(['public', 'auth']).then();
        };
        if (token == null || token.token == null) {
          navigateAuth();
          return false;
        }
        const expireDate = new Date(token.expiryDate ?? '');
        const currentDate = new Date();
        if (expireDate < currentDate) {
          this._authService.signOut().subscribe(() => navigateAuth());
          return false;
        }
        return true;
      })
    );
  }
}
