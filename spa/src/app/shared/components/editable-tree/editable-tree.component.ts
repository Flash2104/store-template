import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
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
  title: string;
  order?: number | null;
  children: IItemNode[];
  parent: IItemNode | null;
}

export interface IItemFlatNode {
  id?: string | number | null | undefined;
  title: string;
  order?: number | null;
  level: number;
  expandable: boolean;
}

@Component({
  selector: 'str-editable-tree',
  templateUrl: './editable-tree.component.html',
  styleUrls: ['./editable-tree.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EditableTreeComponent implements OnInit, OnChanges, OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  // getLevel: (node: IItemFlatNode) => number = (node: IItemFlatNode): number => {
  //   return node.level;
  // };

  // isExpandable: (node: IItemFlatNode) => boolean = (
  //   node: IItemFlatNode
  // ): boolean => {
  //   return node.expandable;
  // };

  // getChildren: (node: IItemNode) => IItemNode[] = (
  //   node: IItemNode
  // ): IItemNode[] => {
  //   return node.children;
  // };

  // /** Map from flat node to nested node. This helps us finding the nested node to be modified */
  // flatNodeMap: Map<IItemFlatNode, IItemNode> = new Map<
  //   IItemFlatNode,
  //   IItemNode
  // >();

  // /** Map from nested node to flattened node. This helps us to keep the same object for selection */
  // nestedNodeMap: Map<IItemNode, IItemFlatNode> = new Map<
  //   IItemNode,
  //   IItemFlatNode
  // >();

  // transformer: (node: IItemNode, level: number) => IItemFlatNode = (
  //   node: IItemNode,
  //   level: number
  // ): IItemFlatNode => {
  //   const existingNode = this.nestedNodeMap.get(node);
  //   const flatNode =
  //     existingNode && existingNode.title === node.title
  //       ? existingNode
  //       : ({} as IItemFlatNode);
  //   flatNode.title = node.title;
  //   flatNode.level = level;
  //   flatNode.expandable = !!node.children?.length;
  //   this.flatNodeMap.set(flatNode, node);
  //   this.nestedNodeMap.set(node, flatNode);
  //   return flatNode;
  // };

  @Input() items: IItemNode[] = [];
  @Output() changed: EventEmitter<IItemNode[]> = new EventEmitter<
    IItemNode[]
  >();

  editItem$: Subject<IItemNode | null> = new Subject<IItemNode | null>();

  treeControl: NestedTreeControl<IItemNode> = new NestedTreeControl<IItemNode>(
    (node) => node.children
  );

  // treeFlattener: MatTreeFlattener<IItemNode, IItemFlatNode> =
  //   new MatTreeFlattener(
  //     this.transformer,
  //     this.getLevel,
  //     this.isExpandable,
  //     this.getChildren
  //   );

  dataSource: MatTreeNestedDataSource<IItemNode> =
    new MatTreeNestedDataSource();

  constructor() {}

  ngOnChanges(changes: SimpleChanges): void {
    if (
      changes.items != null &&
      changes.items.currentValue != changes.items.previousValue
    ) {
      this.dataSource.data = changes.items.currentValue;
    }
  }

  ngOnInit(): void {}

  hasChild(_: number, nodeData: IItemNode): boolean {
    return nodeData.children && nodeData.children.length > 0;
  }

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }

  onEditItem(node: IItemNode): void {
    this.editItem$.next(node);
  }

  drop(event: CdkDragDrop<IItemNode[]>, items: unknown): void {
    moveItemInArray(items as [], event.previousIndex, event.currentIndex);
  }

  // getParent(
  //   node: ICategoryItemData,
  //   getLevel: (x: ICategoryItemData) => number,
  //   dataNodes: ICategoryItemData[]
  // ): ICategoryItemData | null {
  //   const currentLevel = getLevel(node);
  //   if (currentLevel < 1) {
  //     return null;
  //   }

  //   const startIndex = dataNodes.indexOf(node) - 1;

  //   for (let i = startIndex; i >= 0; i--) {
  //     const currentNode = dataNodes[i];

  //     if (getLevel(currentNode) < currentLevel) {
  //       return currentNode;
  //     }
  //   }
  //   return null;
  // }
}
