import { NestedTreeControl } from '@angular/cdk/tree';
import {
  ChangeDetectionStrategy,
  Component,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { MatTreeNestedDataSource } from '@angular/material/tree';
import { Subject } from 'rxjs';

@Component({
  selector: 'str-admin-products',
  templateUrl: './admin-products.component.html',
  styleUrls: ['./admin-products.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AdminProductsComponent implements OnInit, OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  treeControl: NestedTreeControl<any> = new NestedTreeControl<any>(
    (node) => node.children
  );

  dataSource: MatTreeNestedDataSource<any> = new MatTreeNestedDataSource<any>();

  // constructor(
  //   private _navService: NavigationService,
  //   private _navRepo: NavigationRepository
  // ) {}

  // eslint-disable-next-line @angular-eslint/no-empty-lifecycle-method
  ngOnInit(): void {
    //   this._navService.loadUserNavigation().subscribe();
    //   this._navRepo.navData$
    //     .pipe(
    //       tap((navData) => {
    //         if (navData != null) {
    //           const defaultData = navData.find((x) => x.isDefault)?.navItems;
    //           const data = defaultData != null ? this.sortItems(defaultData) : [];
    //           this.dataSource.data = [...data];
    //         }
    //       }),
    //       takeUntil(this._destroy$)
    //     )
    //     .subscribe();
  }

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }

  // hasChild(a: number, node: INavigationItem): boolean {
  //   return !!node.children && node.children.length > 0;
  // }

  // onRouterLinkActive(active: boolean, node: INavigationItem): void {
  //   active ? this.treeControl.expandDescendants(node) : void 0;
  // }

  // sortItems(items: INavigationItem[]): INavigationItem[] {
  //   items.forEach((element) => {
  //     if (element.children != null) {
  //       element.children = this.sortItems(element.children);
  //     }
  //   });
  //   return items.sort((a, b) => a.order - b.order);
  // }
}
