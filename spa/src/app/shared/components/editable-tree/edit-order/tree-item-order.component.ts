import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
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
import { BehaviorSubject, Subject, takeUntil, tap } from 'rxjs';
import { IItemNode } from '../editable-tree.component';

@Component({
  selector: 'str-tree-item-order',
  templateUrl: './tree-item-order.component.html',
  styleUrls: ['./tree-item-order.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TreeItemEditOrderComponent
  implements OnInit, OnChanges, OnDestroy
{
  private _destroy$: Subject<void> = new Subject<void>();

  @Input() items: IItemNode[] | null | undefined = null;
  @Output() selected: EventEmitter<IItemNode> = new EventEmitter<IItemNode>();
  @Output() cancel: EventEmitter<void> = new EventEmitter<void>();
  @Output() changed: EventEmitter<void> = new EventEmitter<void>();

  data$: Subject<IItemNode[] | null> = new Subject<IItemNode[] | null>();
  isChanged$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  constructor() {}

  ngOnInit(): void {
    this.data$
      .pipe(
        tap((x) => console.log(x)),
        takeUntil(this._destroy$)
      )
      .subscribe();
    if (this.items != null && this.items.length > 0) {
      this.data$.next(this.items);
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (
      changes.items != null &&
      changes.items.currentValue !== changes.items.previousValue
    ) {
      this.data$.next(changes.items.currentValue);
    }
  }

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
    this.data$.complete();
  }

  onCancel(): void {
    this.cancel.emit();
  }

  onSelect(item: IItemNode): void {
    this.selected.emit(item);
  }

  drop(event: CdkDragDrop<IItemNode[]>): void {
    moveItemInArray(this.items as [], event.previousIndex, event.currentIndex);
    this.items?.forEach((element, index) => {
      element.order = index + 1;
    });
    this.changed.emit();
  }
}
