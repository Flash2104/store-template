import { NestedTreeControl } from '@angular/cdk/tree';
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
import { MatTreeNestedDataSource } from '@angular/material/tree';
import { Subject } from 'rxjs';

export interface IItemNode {
  id?: string | number | null | undefined;
  tempId?: string | null | undefined;
  title: string;
  order?: number | null;
  icon?: string | null;
  isDisabled?: boolean | null;
  isExpanded?: boolean | null;
  children: IItemNode[];
  parent: IItemNode | null;
}

export interface IChangedEventData {
  removedIds: (string | number)[];
  updatedRoot: IItemNode | null | undefined;
}

@Component({
  selector: 'str-editable-tree',
  templateUrl: './editable-tree.component.html',
  styleUrls: ['./editable-tree.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EditableTreeComponent implements OnInit, OnChanges, OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  @Input() root: IItemNode | null | undefined = null;
  @Output() changed: EventEmitter<IChangedEventData> =
    new EventEmitter<IChangedEventData>();

  editItem: IItemNode | null = null;
  editItemOrder: IItemNode[] | null | undefined = null;

  treeControl: NestedTreeControl<IItemNode> = new NestedTreeControl<IItemNode>(
    (node) => node.children
  );

  dataSource: MatTreeNestedDataSource<IItemNode> =
    new MatTreeNestedDataSource();

  constructor() {}

  ngOnChanges(changes: SimpleChanges): void {
    if (
      changes.root?.currentValue != null &&
      changes.root.currentValue != changes.root.previousValue
    ) {
      const root = changes.root.currentValue;
      this.dataSource.data = root.children;
      this.editItem = null;
      this.editItemOrder = null;
    }
  }

  ngOnInit(): void {
    if (this.root?.children != null) {
      this.dataSource.data =
        this.root?.children != null ? this.root.children : [];
    }
  }

  hasChild(_: number, nodeData: IItemNode): boolean {
    return nodeData.children && nodeData.children.length > 0;
  }

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }

  onEditItem(node: IItemNode): void {
    this.treeControl.expandDescendants(node);
    this.editItem = node;
    this.editItemOrder = node.parent?.children;
  }

  onUp(): void {
    const parent =
      this.editItemOrder != null && this.editItemOrder.length > 0
        ? this.editItemOrder[0].parent?.parent
        : null;
    this.editItemOrder = parent?.children;
    this.editItem =
      this.editItemOrder != null && this.editItemOrder.length > 0
        ? this.editItemOrder[0]
        : null;
    if (parent != null) {
      this.treeControl.expand(parent);
    }
  }

  onDown(): void {
    if (this.editItem != null) {
      this.treeControl.expand(this.editItem);
    }
    this.editItemOrder =
      this.editItem != null && this.editItem.children.length > 0
        ? this.editItem?.children
        : null;
    this.editItem = this.editItemOrder != null ? this.editItemOrder[0] : null;
  }

  reRenderTree(removedIds: (string | number)[] = []): void {
    const data = this.dataSource.data;
    this.dataSource.data = [];
    this.dataSource.data = data;
    if (this.root != null) {
      this.root.children = data;
      this.changed.emit({
        removedIds,
        updatedRoot: this.root,
      });
    }
  }

  onAddChild(node: IItemNode): void {
    node.children = node.children == null ? [] : node.children;
    const newNode: IItemNode = {
      id: 0,
      children: [],
      parent: node,
      title: 'Новая категория',
      isDisabled: false,
      tempId: uuidv1(),
      order: node.children.length + 1,
    };
    node.children.push(newNode);
    this.editItem = newNode;
    this.editItemOrder = newNode.parent?.children;
    this.treeControl.expand(node);
    this.reRenderTree();
  }

  onRemove(node: IItemNode): void {
    const index = node.parent?.children?.indexOf(node);
    const removedIds: (string | number)[] = node.id != null ? [node.id] : [];
    const descendants = this.treeControl.getDescendants(node);
    descendants.forEach((el) => {
      if (el.id != null) {
        removedIds.push(el.id);
      }
    });
    if (index != null && index > -1) {
      node.parent?.children?.splice(index, 1);
    }
    node.parent?.children?.forEach((element, index) => {
      element.order = index + 1;
    });
    if (node === this.editItem) {
      this.editItem = null;
      this.editItemOrder = null;
    }
    const temp = this.editItemOrder;
    this.editItemOrder = null;
    this.editItemOrder = temp;
    this.reRenderTree(removedIds);
  }

  getButtonColor(node: IItemNode): string | null {
    if (node === this.editItem) {
      return 'accent';
    }
    if (node.isDisabled) {
      return null;
    }
    return 'primary';
  }

  disableAllDescendants(node: IItemNode): void {
    const descendants = this.treeControl.getDescendants(node);
    descendants.forEach((el) => {
      el.isDisabled = node.isDisabled;
    });
    this.reRenderTree();
  }
}
function uuidv1(): string | null | undefined {
  throw new Error('Function not implemented.');
}

