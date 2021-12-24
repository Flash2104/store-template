import { ChangeDetectionStrategy, Component, OnDestroy } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { AuthService } from '../../../../../shared/services/auth.service';
import { FormErrorStateMatcher } from '../../../../../shared/utils/error-state-matcher';

export interface ISignUpData {
  phoneOrEmail: string;
  password: string;
  confirmPassword: string;
}

@Component({
  selector: 'str-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SignUpComponent implements OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  form: FormGroup = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [
      Validators.required,
      Validators.minLength(6),
    ]),
    confirmPassword: new FormControl('', [
      Validators.required,
      Validators.minLength(6),
    ]),
  });

  matcher: FormErrorStateMatcher = new FormErrorStateMatcher();

  constructor(private _authService: AuthService) {}

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }

  onSubmit(): void {
    if (this.form.valid) {
      this._authService
        .signUp({
          phoneOrEmail: this.form.controls['email'].value,
          password: this.form.controls['password'].value,
          confirmPassword: this.form.controls['confirmPassword'].value,
        })
        .subscribe();
    }
  }
}
