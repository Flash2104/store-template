/* eslint-disable @typescript-eslint/no-empty-function */
import {
  ChangeDetectionStrategy,
  Component,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import {
  filter,
  map,
  Observable,
  of,
  Subject,
  switchMap,
  takeUntil,
} from 'rxjs';
import { ProfileService } from 'src/app/shared/services/profile.service';
import { ProfileRepository } from '../../shared/repository/profile.repository';
import { IProfileData } from '../../shared/services/dto-models/profile/profile-data';

@Component({
  selector: 'air-profile-main-page',
  templateUrl: './profile-main-page.component.html',
  styleUrls: ['./profile-main-page.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProfileMainPageComponent implements OnInit, OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  public loading$: Observable<boolean> = this._profileRepo.loading$.pipe(
    takeUntil(this._destroy$)
  );

  public profile$: Observable<IProfileData | null> =
    this._profileRepo.profile$.pipe(
      filter((p) => p != null),
      map((p) => {
        const sanitized = this._sanitizer.bypassSecurityTrustResourceUrl(
          'data:image/png;base64, ' + p?.avatarData
        );
        return { ...p, avatarData: sanitized } as IProfileData;
      }),
      takeUntil(this._destroy$)
    );

  constructor(
    private _profileRepo: ProfileRepository,
    private _profileService: ProfileService,
    private _sanitizer: DomSanitizer
  ) {}

  // eslint-disable-next-line @angular-eslint/no-empty-lifecycle-method
  ngOnInit(): void {
    this._profileRepo.profile$
      .pipe(
        switchMap((p) => {
          if (p == null) {
            return this._profileService.loadCurrentProfile();
          }
          return of(0);
        }),
        takeUntil(this._destroy$)
      )
      .subscribe();
  }

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }
}
