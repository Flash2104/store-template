import { NestedTreeControl } from '@angular/cdk/tree';
import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  EventEmitter,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  Output,
  SimpleChanges,
  ViewChild,
} from '@angular/core';
import { MatTree, MatTreeNestedDataSource } from '@angular/material/tree';
import { Subject } from 'rxjs';

export interface IItemNode {
  id?: string | number | null | undefined;
  title: string;
  order?: number | null;
  icon?: string | null;
  isDisabled?: boolean | null;
  children: IItemNode[];
  parent: IItemNode | null;
}

@Component({
  selector: 'str-editable-tree',
  templateUrl: './editable-tree.component.html',
  styleUrls: ['./editable-tree.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EditableTreeComponent implements OnInit, OnChanges, OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  @Input() items: IItemNode[] | null = null;
  @Output() changed: EventEmitter<IItemNode[]> = new EventEmitter<
    IItemNode[]
  >();

  editItem: IItemNode | null = null;
  editItemOrder: IItemNode[] | null | undefined = null;

  treeControl: NestedTreeControl<IItemNode> = new NestedTreeControl<IItemNode>(
    (node) => node.children
  );

  dataSource: MatTreeNestedDataSource<IItemNode> =
    new MatTreeNestedDataSource();

  @ViewChild('treeSelector', { static: false })
  tree: MatTree<IItemNode> | null = null;

  constructor(private _cd: ChangeDetectorRef) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (
      changes.items?.currentValue != null &&
      changes.items.currentValue != changes.items.previousValue
    ) {
      this.dataSource.data = changes.items.currentValue;
      this.editItem = null;
      this.editItemOrder = null;
    }
  }

  ngOnInit(): void {
    if (this.items != null) {
      this.dataSource.data = this.items;
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
    this.editItem = node;
    this.editItemOrder = node.parent?.children;
  }

  onOrderChanged(): void {
    // this.tree?.renderNodeChanges(this.dataSource.data);
    const data = this.dataSource.data;
    this.dataSource.data = [];
    this.dataSource.data = data;
  }

  trackByFn(index: number, node: IItemNode): number | null | undefined {
    return node.order;
  }
}
