import { OverlayContainer } from '@angular/cdk/overlay';
import { ChangeDetectionStrategy, Component, OnDestroy, OnInit, Renderer2 } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { Subject, takeUntil } from 'rxjs';
import { ProfileRepository } from 'src/app/shared/repository/profile.repository';
import { AuthService } from 'src/app/shared/services/auth.service';

@Component({
  selector: 'air-public-toolbar',
  templateUrl: './public-toolbar.component.html',
  styleUrls: ['./public-toolbar.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PublicToolbarComponent implements OnInit, OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  form: FormGroup = new FormGroup({
    toggleControl: new FormControl(false),
  });

  constructor(
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

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }
}
