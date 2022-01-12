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
import { Subject } from 'rxjs';
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
  @Output() confirm: EventEmitter<IItemNode> = new EventEmitter<IItemNode>();

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
      this.form.controls.title.patchValue(currVal.title);
      this.form.controls.icon.patchValue(currVal.icon);
      this.form.controls.isDisabled.patchValue(currVal.isDisabled);
    }
  }

  ngOnInit(): void {
    if (this.item != null) {
      this.form.controls.title.patchValue(this.item.title);
      this.form.controls.icon.patchValue(this.item.icon);
      this.form.controls.isDisabled.patchValue(this.item.isDisabled);
    }
  }

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }

  onCancel(): void {
    this.cancel.emit();
  }

  onConfirm(): void {
    this.item.title = this.form.controls.title.value;
    this.confirm.emit(this.item);
  }
}
