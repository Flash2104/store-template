import { ShopService } from './shared/services/shop.service';
import {
  animate,
  animateChild,
  group,
  query,
  style,
  transition,
  trigger,
} from '@angular/animations';
import {
  ChangeDetectionStrategy,
  Component,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { RouterOutlet } from '@angular/router';
import { filter, Subject, takeUntil } from 'rxjs';
import { AuthRepository } from './shared/repository/auth.repository';
import { ShopRepository } from './shared/repository/shop.repository';
import { AuthService } from './shared/services/auth.service';

export const slideInAnimation = trigger('routeAnimations', [
  transition('StorePages <=> AdminPages', [
    style({ position: 'relative' }),
    query(':enter, :leave', [
      style({
        position: 'absolute',
        top: 0,
        left: 0,
        width: '100%',
      }),
    ]),
    query(':enter', [style({ left: '-100%' })]),
    query(':leave', animateChild()),
    group([
      query(':leave', [animate('400ms ease-out', style({ left: '100%' }))]),
      query(':enter', [animate('400ms ease-out', style({ left: '0%' }))]),
    ]),
    query(':enter', animateChild()),
  ]),
]);

@Component({
  selector: 'str-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  animations: [slideInAnimation],
})
export class AppComponent implements OnInit, OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();
  form: FormGroup = new FormGroup({
    toggleControl: new FormControl(false),
  });

  constructor(
    public authService: AuthService,
    private _authRepo: AuthRepository,
    private _shopRepo: ShopRepository,
    private _shopService: ShopService
  ) {}

  ngOnInit(): void {
    this._shopService.loadShopInfo().subscribe();
    this._authRepo.token$
      .pipe(
        filter((t) => t != null),
        takeUntil(this._destroy$)
      )
      .subscribe();
  }

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();

    this._shopRepo.destroy();
  }

  prepareRoute(outlet: RouterOutlet): string {
    return outlet?.activatedRouteData?.['animation'];
  }
}
