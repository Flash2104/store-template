import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  SimpleChanges,
} from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { debounceTime, filter, Subject, takeUntil, tap } from 'rxjs';
import { IItemNode } from '../editable-tree.component';

@Component({
  selector: 'str-tree-item-edit',
  templateUrl: './tree-item-edit.component.html',
  styleUrls: ['./tree-item-edit.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TreeItemEditComponent implements OnInit, OnChanges, OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  @Input() item!: IItemNode;
  @Output() cancel: EventEmitter<void> = new EventEmitter<void>();
  @Output() changed: EventEmitter<IItemNode> = new EventEmitter<IItemNode>();
  @Output() disabled: EventEmitter<IItemNode> = new EventEmitter<IItemNode>();

  form: FormGroup = new FormGroup({
    title: new FormControl(null, [Validators.required]),
    icon: new FormControl(null),
    isDisabled: new FormControl(false),
  });

  constructor() {}
  ngOnChanges(changes: SimpleChanges): void {
    if (
      changes.item?.currentValue != null &&
      changes.item.currentValue !== changes.item.previousValue
    ) {
      const currVal = changes.item.currentValue;
      this.form.controls.title.setValue(currVal.title, { emitEvent: false });
      this.form.controls.icon.setValue(currVal.icon, { emitEvent: false });
      this.form.controls.isDisabled.setValue(currVal.isDisabled, {
        emitEvent: false,
      });
    }
  }

  ngOnInit(): void {
    if (this.item != null) {
      this.form.controls.title.patchValue(this.item.title);
      this.form.controls.icon.patchValue(this.item.icon);
      this.form.controls.isDisabled.patchValue(this.item.isDisabled);
    }
    this.form.controls.title.valueChanges
      .pipe(
        debounceTime(300),
        filter((v) => v != null && v !== ''),
        tap((v) => {
          this.item.title = v;
          this.changed.emit(this.item);
        }),
        takeUntil(this._destroy$)
      )
      .subscribe();

    this.form.controls.icon.valueChanges
      .pipe(
        debounceTime(300),
        tap((v) => {
          this.item.icon = v;
          this.changed.emit(this.item);
        }),
        takeUntil(this._destroy$)
      )
      .subscribe();
    this.form.controls.isDisabled.valueChanges
      .pipe(
        debounceTime(300),
        tap((v) => {
          this.item.isDisabled = v;
          this.disabled.emit(this.item);
        }),
        takeUntil(this._destroy$)
      )
      .subscribe();
  }

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }

  onCancel(): void {
    this.cancel.emit();
  }
}
