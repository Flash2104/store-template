import {
  ChangeDetectionStrategy,
  Component,
  OnDestroy,
  OnInit,
  Renderer2,
} from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { filter, map, Observable, Subject, takeUntil } from 'rxjs';
import { ProfileRepository } from 'src/app/shared/repository/profile.repository';
import { IProfileData } from 'src/app/shared/services/dto-models/profile/profile-data';
import { FormGroup, FormControl } from '@angular/forms';
import { OverlayContainer } from '@angular/cdk/overlay';
import { AuthService } from '../shared/services/auth.service';

@Component({
  selector: 'str-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ToolbarComponent implements OnInit, OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();
  public profile$: Observable<IProfileData | null> =
    this._profileRepo.profile$.pipe(
      filter((p) => p != null),
      map((p) => {
        const sanitized = this._sanitizer.bypassSecurityTrustResourceUrl(
          'data:image/png;base64, ' + p?.avatarIcon
        );
        return { ...p, avatarIcon: sanitized } as IProfileData;
      }),
      takeUntil(this._destroy$)
    );

  form: FormGroup = new FormGroup({
    toggleControl: new FormControl(false),
  });

  constructor(
    private _sanitizer: DomSanitizer,
    private _profileRepo: ProfileRepository,
    private _authService: AuthService,
    private _overlay: OverlayContainer,
    private _renderer: Renderer2,
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
}
