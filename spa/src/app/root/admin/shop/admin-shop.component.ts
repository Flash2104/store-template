import {
  IShopData,
  ShopRepository,
} from './../../../shared/repository/shop.repository';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import {
  ChangeDetectionStrategy,
  Component,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { Subject, Observable, takeUntil, map } from 'rxjs';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

@Component({
  selector: 'str-admin-shop',
  templateUrl: './admin-shop.component.html',
  styleUrls: ['./admin-shop.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AdminShopComponent implements OnInit, OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  loading$: Observable<boolean> = this._shopRepo.loading$.pipe(
    takeUntil(this._destroy$)
  );

  shopOrigin$: Observable<IShopData | null> = this._shopRepo.origin$.pipe(
    map((shop) => {
      const sanitized: SafeResourceUrl | null =
        shop?.logo != null
          ? this._sanitizer.bypassSecurityTrustResourceUrl(
              'data:image/png;base64, ' + shop.logo
            )
          : null;
      return {
        ...shop,
        logo: sanitized as string | null,
      };
    }),
    takeUntil(this._destroy$)
  );

  form: FormGroup = new FormGroup({
    title: new FormControl(null, [Validators.required]),
    logo: new FormControl(null),
  });

  constructor(
    private _shopRepo: ShopRepository,
    private _sanitizer: DomSanitizer
  ) {}

  ngOnInit(): void {}

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }

  onCancel(): void {
    this._shopRepo.resetChanged();
  }
}
