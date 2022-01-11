import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
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
export class TreeItemEditComponent implements OnInit, OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  @Input() item!: IItemNode;
  @Output() cancel: EventEmitter<void> = new EventEmitter<void>();
  @Output() confirm: EventEmitter<IItemNode> = new EventEmitter<IItemNode>();

  form: FormGroup = new FormGroup({
    title: new FormControl(null, [Validators.required]),
    icon: new FormControl(false),
    isDisabled: new FormControl(null),
  });

  constructor() {}

  ngOnInit(): void {}

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
