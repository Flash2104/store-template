import { ShopService } from './../../../shared/services/shop.service';
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
import { Subject, Observable, takeUntil, map, tap, filter } from 'rxjs';
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

  shopChanged$: Observable<IShopData | null> = this._shopRepo.changed$.pipe(
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

  isChanged$: Observable<boolean> = this._shopRepo.isChanged$.pipe(
    takeUntil(this._destroy$)
  );

  form: FormGroup = new FormGroup({
    title: new FormControl(null, [Validators.required]),
    logo: new FormControl(null),
  });

  constructor(
    private _shopRepo: ShopRepository,
    private _shopService: ShopService,
    private _sanitizer: DomSanitizer
  ) {}

  ngOnInit(): void {
    this.form.controls.title.valueChanges.pipe(
      tap((v) => this._shopRepo.changeTitle(v)),
      takeUntil(this._destroy$)
    ).subscribe();

    this.shopChanged$.pipe(
      filter(v => v != null),
      tap(v => v != null && this.form.patchValue(v, {emitEvent: false})),
      takeUntil(this._destroy$)
    ).subscribe();
  }

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }

  onCancel(): void {
    this._shopRepo.resetChanged();
  }

  onLogoChange(): void {
    console.log('Logo changed');
  }

  onSave(): void {
    this._shopService.updateShopInfo().subscribe();
  }
}
