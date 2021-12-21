import { IReferenceData } from './../../services/dto-models/reference-data';
import {
  ChangeDetectionStrategy,
  Component,
  forwardRef,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  SimpleChanges
} from '@angular/core';
import {ControlValueAccessor, NG_VALUE_ACCESSOR} from '@angular/forms';

@Component({
  selector: 'air-selector',
  templateUrl: './air-selector.component.html',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => AirSelectorComponent),
      multi: true
    }
  ],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AirSelectorComponent implements ControlValueAccessor, OnInit, OnDestroy, OnChanges {

  private _value: IReferenceData<string | number> | null = null;

  get value(): IReferenceData<string | number> | null {
    return this._value;
  }

  @Input() data: IReferenceData<string | number>[] | null = [];
  @Input() loading: boolean = false;
  @Input() filterable: boolean = false;
  @Input() placeholder: string = '';
  @Input() disabled: boolean = false;
  @Input() startsWithFilter: boolean = false;

  public source: IReferenceData<string | number>[] = [];

  constructor() {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if(changes.data?.currentValue != null && changes.data.currentValue !== changes.data.previousValue) {
      this.source = changes.data.currentValue;
    }
  }

  ngOnInit(): void {
    this.source = this.data != null ? this.data.slice() : [];
  }

  // eslint-disable-next-line @angular-eslint/no-empty-lifecycle-method
  ngOnDestroy(): void {  }

  onTouched: () => void = () => { };

  // eslint-disable-next-line @typescript-eslint/no-empty-function
  propagateChange: (value: IReferenceData<string | number>) => void = (value: IReferenceData<string | number>) => { };

  writeValue(obj: IReferenceData<string | number>): void {
    this._value = obj;
  }

  // eslint-disable-next-line @typescript-eslint/no-explicit-any,  @typescript-eslint/explicit-module-boundary-types
  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  // eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
  registerOnChange(fn: (value: IReferenceData<string | number>) => void): void {
    this.propagateChange = fn;
  }

  // eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
  onChange(newValue: IReferenceData<string | number>): void {
    this.propagateChange(newValue);
  }

  onFilterChanged(value: string): void {
    if (this.data != null) {
      if (this.startsWithFilter) {
        this.source = this.data.filter(
          (s) => s.title != null && s.title.toLowerCase().startsWith(value.toLowerCase())
        );
      }
      else {
        this.source = this.data.filter(
          (s) => s.title != null && s.title.toLowerCase().indexOf(value.toLowerCase()) !== -1
        );
      }
    }
  }
}
