import { ShopService } from './../shared/services/shop.service';
import {
  ShopRepository,
  IShopData,
} from './../shared/repository/shop.repository';
import { OverlayContainer } from '@angular/cdk/overlay';
import {
  ChangeDetectionStrategy,
  Component,
  OnDestroy,
  OnInit,
  Renderer2,
} from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { Subject, takeUntil, Observable, map, filter } from 'rxjs';
import { AuthService } from '../shared/services/auth.service';

@Component({
  selector: 'str-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ToolbarComponent implements OnInit, OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  shopInfo$: Observable<IShopData | null> = this._shopRepo.origin$.pipe(
    filter((si) => si != null),
    map((shopInfo) => {
      let sanitized: SafeResourceUrl | null = null;
      if (shopInfo?.logo) {
        sanitized = this._sanitizer.bypassSecurityTrustResourceUrl(
          'data:image/png;base64, ' + shopInfo?.logo
        );
      }
      return { ...shopInfo, logo: sanitized } as IShopData;
    }),
    takeUntil(this._destroy$)
  );

  shopInfoLoading$: Observable<boolean> = this._shopRepo.loading$.pipe(
    takeUntil(this._destroy$)
  );

  form: FormGroup = new FormGroup({
    toggleControl: new FormControl(false),
  });

  constructor(
    private _sanitizer: DomSanitizer,
    private _authService: AuthService,
    private _overlay: OverlayContainer,
    private _renderer: Renderer2,
    private _shopRepo: ShopRepository,
    private _shopService: ShopService
  ) {}

  ngOnInit(): void {
    this.form.controls['toggleControl'].valueChanges
      .pipe(takeUntil(this._destroy$))
      .subscribe((lightMode) => {
        const lightClassName = 'light-theme';
        if (lightMode) {
          this._renderer.addClass(document.body, lightClassName);
          this._overlay.getContainerElement().classList.add(lightClassName);
        } else {
          this._renderer.removeClass(document.body, lightClassName);
          this._overlay.getContainerElement().classList.remove(lightClassName);
        }
      });
  }

  onToggle(): void {
    const toggleControl = this.form.controls['toggleControl'];
    toggleControl.setValue(!toggleControl.value);
  }

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }

  onLogout(): void {
    this._authService.signOut().subscribe();
  }

  reloadInfo(): void {
    this._shopService.loadShopInfo().subscribe();
  }
}
