import {
  IReferenceData,
  IGroupedReferenceData,
} from '../../services/dto-models/reference-data';
import {
  ChangeDetectionStrategy,
  Component,
  forwardRef,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  SimpleChanges,
} from '@angular/core';
import {
  ControlValueAccessor,
  FormControl,
  NG_VALUE_ACCESSOR,
} from '@angular/forms';
import {
  debounceTime,
  ReplaySubject,
  Subject,
  takeUntil,
  tap,
} from 'rxjs';

@Component({
  selector: 'str-grouped-selector',
  templateUrl: './grouped-selector.component.html',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => GroupedSelectorComponent),
      multi: true,
    },
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class GroupedSelectorComponent
  implements ControlValueAccessor, OnInit, OnDestroy, OnChanges
{
  private _destroy$: Subject<void> = new Subject<void>();
  private _value: IReferenceData<string | number> | null = null;

  get value(): IReferenceData<string | number> | null {
    return this._value;
  }

  @Input() data: IGroupedReferenceData<string | number>[] = [];
  @Input() loading: boolean = false;
  @Input() placeholder: string = '';
  @Input() disabled: boolean = false;


  public filterCntr: FormControl = new FormControl();
  public filteredGroups$: ReplaySubject<
    IGroupedReferenceData<string | number>[]
  > = new ReplaySubject<IGroupedReferenceData<string | number>[]>(1);

  constructor() {}

  ngOnChanges(changes: SimpleChanges): void {
    if (
      changes.data?.currentValue != null &&
      changes.data.currentValue !== changes.data.previousValue
    ) {
      this.filteredGroups$.next(this.copyGroups(changes.data.currentValue)); ;
    }
  }

  ngOnInit(): void {
    this.filteredGroups$.next(this.copyGroups(this.data)); ;
    this.filterCntr.valueChanges
      .pipe(
        debounceTime(500),
        tap((filter) => this.filterDataSource(filter)),
        takeUntil(this._destroy$)
      )
      .subscribe();
  }

  // eslint-disable-next-line @angular-eslint/no-empty-lifecycle-method
  ngOnDestroy(): void {}

  onTouched: () => void = () => {};

  // eslint-disable-next-line @typescript-eslint/no-empty-function
  propagateChange: (value: IReferenceData<string | number>) => void = (
    value: IReferenceData<string | number>
  ) => {};

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

  private filterDataSource(search: string): void {
    if (this.data.length === 0) {
      return;
    }
    const groupsCopy = this.copyGroups(this.data);
    if (!search) {
      this.filteredGroups$.next(groupsCopy);
      return;
    } else {
      search = search.toLowerCase();
    }
    // filter the banks
    this.filteredGroups$.next(
      groupsCopy.filter(group => {
        const showGroup = group.key.toLowerCase().indexOf(search) > -1;
        if (!showGroup) {
          group.data = group.data.filter(el => el.title.toLowerCase().indexOf(search) > -1);
        }
        return group.data.length > 0;
      })
    );
  }

  private copyGroups(groups: IGroupedReferenceData<string | number>[]): IGroupedReferenceData<string | number>[] {
    const groupsCopy: IGroupedReferenceData<string | number>[] = [];
    groups.forEach((group) => {
      groupsCopy.push({
        key: group.key,
        data: group.data.slice(),
      });
    });
    return groupsCopy;
  }
}
