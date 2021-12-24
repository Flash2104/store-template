import { ChangeDetectionStrategy, Component, OnDestroy } from '@angular/core';
import { Observable, Subject, takeUntil } from 'rxjs';
import { AuthRepository } from '../../../../shared/repository/auth.repository';

@Component({
  selector: 'str-auth-container',
  templateUrl: './auth-container.component.html',
  styleUrls: ['./auth-container.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AuthContainerComponent implements OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  public authLoading$: Observable<boolean> = this._authRepo.loading$.pipe(
    takeUntil(this._destroy$)
  );

  constructor(private _authRepo: AuthRepository) {}

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }
}
