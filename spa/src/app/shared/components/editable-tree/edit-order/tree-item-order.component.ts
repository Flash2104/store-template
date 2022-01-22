import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import {
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  Output,
} from '@angular/core';
import { Subject } from 'rxjs';
import { IItemNode } from '../editable-tree.component';

// eslint-disable-next-line change-detection-strategy/on-push
@Component({
  selector: 'str-tree-item-order',
  templateUrl: './tree-item-order.component.html',
  styleUrls: ['./tree-item-order.component.scss']
})
export class TreeItemEditOrderComponent implements OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  @Input() items: IItemNode[] | null | undefined = null;
  @Input() selected: IItemNode | null | undefined = null;
  @Output() selectedEvent: EventEmitter<IItemNode> =
    new EventEmitter<IItemNode>();

  @Output() cancel: EventEmitter<void> = new EventEmitter<void>();
  @Output() upEvent: EventEmitter<void> = new EventEmitter<void>();
  @Output() downEvent: EventEmitter<void> = new EventEmitter<void>();
  @Output() changed: EventEmitter<void> = new EventEmitter<void>();

  constructor() {}

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }

  canUp(): boolean {
    return (
      this.items != null &&
      this.items.length > 0 &&
      this.items[0].parent?.parent != null
    );
  }

  canDown(): boolean {
    return (
      this.selected != null &&
      this.selected.children != null &&
      this.selected.children.length > 0
    );
  }

  onCancel(): void {
    this.cancel.emit();
  }

  onUp(): void {
    this.upEvent.emit();
  }

  onDown(): void {
    this.downEvent.emit();
  }

  onSelect(item: IItemNode): void {
    this.selectedEvent.emit(item);
  }

  drop(event: CdkDragDrop<IItemNode[]>): void {
    moveItemInArray(this.items as [], event.previousIndex, event.currentIndex);
    this.items?.forEach((element, index) => {
      element.order = index + 1;
    });
    this.changed.emit();
  }
}
