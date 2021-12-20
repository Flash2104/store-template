/* eslint-disable @typescript-eslint/no-empty-function */
import {
  ChangeDetectionStrategy,
  Component,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { filter, map, Observable, Subject, takeUntil } from 'rxjs';
import { ProfileRepository } from '../../../shared/repository/profile.repository';
import { IProfileData } from '../../../shared/services/dto-models/profile/profile-data';

@Component({
  selector: 'air-profile-main-info-edit',
  templateUrl: './profile-main-info-edit.component.html',
  styleUrls: ['./profile-main-info-edit.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProfileMainInfoEditComponent implements OnInit, OnDestroy {
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
    private _sanitizer: DomSanitizer
  ) {}

  // eslint-disable-next-line @angular-eslint/no-empty-lifecycle-method
  ngOnInit(): void {}

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }
}
