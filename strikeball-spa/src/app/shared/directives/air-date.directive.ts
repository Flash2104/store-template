/* eslint-disable @typescript-eslint/naming-convention */
import {
  Directive,
  ElementRef,
  forwardRef,
  HostListener,
  OnInit,
  Renderer2,
} from '@angular/core';
import {
  ControlValueAccessor,
  NG_VALIDATORS,
  NG_VALUE_ACCESSOR,
} from '@angular/forms';
// import { coalesce } from 'src/shared/helpers/utils-functions';

@Directive({
  selector: 'input[airDate]',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => AirDateDirective),
      multi: true,
    },
    {
      provide: NG_VALIDATORS,
      useExisting: forwardRef(() => AirDateDirective),
      multi: true,
    },
  ],
})
export class AirDateDirective implements OnInit, ControlValueAccessor {
  private onChangedFn!: (date: string | null) => void;
  private onTouchedFn!: () => void;

  private _inputRegex: RegExp = /[^0-9]*/g;
  private _maskChar: string = '_';
  private _nullValue: (string | null)[] = [
    null,
    null,
    '.',
    null,
    null,
    '.',
    null,
    null,
    null,
    null,
  ];

  private _currentValue: (string | null)[] = [...this._nullValue];
  private _maxLength: number = 10;
  private _dateValidRegex: string[] = ['\\d{2}\\.\\d{2}\\.\\d{4}$', 'g'];
  private _valueOnFocus: string | null = null;

  constructor(private renderer: Renderer2, private elRef: ElementRef) {}

  ngOnInit(): void {
    this.renderer.setAttribute(
      this.elRef.nativeElement,
      'maxlength',
      this._maxLength.toString()
    );
  }

  @HostListener('input', ['$event'])
  onInputChange(event: KeyboardEvent): void {
    const elem = this.elRef.nativeElement as HTMLInputElement;
    if (elem == null || elem.selectionStart == null) {
      return;
    }
    const newPosition = [3, 6].includes(elem.selectionStart)
      ? elem.selectionStart - 1
      : elem.selectionStart;
    elem.value = this.getFinalText();
    this.setCursorPosition(newPosition);
  }

  @HostListener('keypress', ['$event'])
  onKeyPress(event: KeyboardEvent): void {
    const elem = this.elRef.nativeElement as HTMLInputElement;
    if (elem == null || elem.selectionStart == null) {
      return;
    }
    // do not allow to enter symbols except for digits
    /* tslint:disable */
    // noinspection JSDeprecatedSymbols
    const key = String.fromCharCode(event.which);
    /* tslint:enable */
    const replacedValue = key.replace(this._inputRegex, '');
    if (replacedValue !== key || elem.selectionStart > this._maxLength - 1) {
      event.preventDefault();
      return;
    }
    if (
      elem == null ||
      elem.selectionStart == null ||
      elem.selectionEnd == null
    ) {
      return;
    }
    const selectionStart = elem.selectionStart;
    this.clearValueAccordingToSelection(selectionStart, elem.selectionEnd);

    const caretPos = [2, 5].includes(selectionStart)
      ? selectionStart + 1
      : selectionStart;
    this._currentValue[caretPos] = key;

    const newPosition = caretPos + 1;
    elem.value = this.getFinalText();
    this.setCursorPosition(newPosition);

    // if final text is a valid date, emit value
    this.onChangedFn(
      elem.value === '' ? null : this.generateOutputIsoValue(elem.value)
    );
    this._valueOnFocus = elem.value;
  }

  @HostListener('keydown', ['$event'])
  onKeyDown(event: KeyboardEvent): void {
    if (this.onTouchedFn != null) {
      this.onTouchedFn();
    }
    const elem = this.elRef.nativeElement as HTMLInputElement;
    if (
      elem == null ||
      elem.selectionStart == null ||
      elem.selectionEnd == null
    ) {
      return;
    }
    if (['Backspace', 'Delete'].includes(event.code)) {
      if (elem.selectionStart !== elem.selectionEnd) {
        const selectionStart = elem.selectionStart;
        this.clearValueAccordingToSelection(
          elem.selectionStart,
          elem.selectionEnd
        );
        elem.value = this.getFinalText();
        this.setCursorPosition(selectionStart);
        event.preventDefault();
      } else {
        if (['Backspace'].includes(event.code)) {
          const caretPos = [3, 6].includes(elem.selectionStart)
            ? elem.selectionStart - 2
            : elem.selectionStart - 1;
          this._currentValue[caretPos] = null;
        } else if (['Delete'].includes(event.code)) {
          // if caret stand before the dot - do nothing
          if ([2, 5].includes(elem.selectionStart)) {
            event.preventDefault();
          } else {
            this._currentValue[elem.selectionStart] = null;
          }
        }
      }
    }
  }

  @HostListener('blur')
  onBlur(): void {
    const elem = this.elRef.nativeElement as HTMLInputElement;

    let finalText = coalesce(this.getFinalText(), '') || '';

    if (this._currentValue.join('') === this._nullValue.join('')) {
      elem.value = '';
      finalText = '';
    } else {
      elem.value = finalText;
    }

    if (this.onChangedFn != null && this._valueOnFocus !== finalText) {
      if (this.onTouchedFn != null) {
        this.onTouchedFn();
      }
      this.onChangedFn(
        finalText === '' ? null : this.generateOutputIsoValue(finalText)
      );
    }

    this._valueOnFocus = null;
  }

  @HostListener('focus')
  onFocus(): void {
    const elem = this.elRef.nativeElement as HTMLInputElement;
    this._valueOnFocus = elem.value;
  }

  @HostListener('paste', ['$event'])
  onPaste(event: ClipboardEvent): void {
    if (event.clipboardData == null) {
      return;
    }
    const clipboardData: string = event.clipboardData.getData('text');
    if (/\d{2}\.\d{2}\.\d{4}$/.test(clipboardData)) {
      this.writeValue(this.generateOutputIsoValue(clipboardData));
    }
  }

  registerOnChange(fn: (data: string | null) => void): void {
    this.onChangedFn = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouchedFn = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    const elem = this.elRef.nativeElement as HTMLInputElement;
    elem.disabled = isDisabled;
  }

  writeValue(obj: string): void {
    const elem = this.elRef.nativeElement as HTMLInputElement;
    const parsedTime = this.parseIsoTime(obj);
    if (parsedTime != null) {
      this._currentValue = Array.from(parsedTime);
      elem.value = this.getFinalText();
    } else {
      this._currentValue = [...this._nullValue];
      elem.value = obj;
    }
  }

  private getFinalText(): string {
    return this._currentValue
      .map((char) => (char === null ? this._maskChar : char))
      .join('');
  }

  private setCursorPosition(position: number): void {
    const elem = this.elRef.nativeElement as HTMLInputElement;
    elem.setSelectionRange(position, position);
  }

  /**
   * converts yyyy-MM-dd to dd.MM.yyyy
   */
  private parseIsoTime(time: string): string | null {
    if (time != null && time.length >= 10) {
      const truncatedString = time.substring(0, 10);
      const regex = /[\d_]{4}-[\d_]{2}-[\d_]{2}/gi;
      if (regex.test(truncatedString)) {
        const arr = truncatedString.split('-');
        return [2, 1, 0].map((i) => arr[i]).join('.');
      }
    }
    return null;
  }

  private clearValueAccordingToSelection(
    selectionStart: number,
    selectionEnd: number
  ): void {
    if (selectionStart === selectionEnd) {
      return;
    }
    const indicesToReplace = [...Array(selectionEnd - selectionStart).keys()]
      .map((idx: number) => idx + selectionStart)
      // exclude dots from selection
      .filter((idx) => [2, 5].includes(idx) === false);

    indicesToReplace.forEach((idx) => (this._currentValue[idx] = null));
  }

  /**
   * return value in iso format
   */
  private generateOutputIsoValue(date: string): string {
    return (
      date.substring(6, 10) +
      '-' +
      date.substring(3, 5) +
      '-' +
      date.substring(0, 2) +
      'T00:00:00'
    );
  }
}

export function coalesce<T>(
  p1: T,
  p2?: T,
  p3?: T,
  p4?: T,
  p5?: T,
  p6?: T,
  p7?: T
): T | null;
export function coalesce<T, T1>(p1: T, p2: T1): T | T1 | null;
export function coalesce<T, T1, T2>(p1: T, p2: T1, p3: T2): T | T1 | T2 | null;
export function coalesce<T>(...params: (T | null | undefined)[]): T | null {
  if (params == null) {
    return null;
  }
  if (params.length === 0) {
    return null;
  }
  const result = params.find((el) => el != null);
  return result != null ? result : null;
}
