import { NestedTreeControl } from '@angular/cdk/tree';
import {
  ChangeDetectionStrategy,
  Component,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { MatTreeNestedDataSource } from '@angular/material/tree';
import { Subject, takeUntil, tap } from 'rxjs';
import { NavigationRepository } from '../shared/repository/navigation.repository';
import { INavigationItem } from '../shared/services/dto-models/navigations/navigation-data';
import { NavigationService } from '../shared/services/navigation.service';

@Component({
  selector: 'str-sidenav-container',
  templateUrl: './sidenav-container.component.html',
  styleUrls: ['./sidenav-container.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SideNavContainerComponent implements OnInit, OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  treeControl: NestedTreeControl<INavigationItem> =
    new NestedTreeControl<INavigationItem>((node) => node.children);

  dataSource: MatTreeNestedDataSource<INavigationItem> =
    new MatTreeNestedDataSource<INavigationItem>();

  constructor(
    private _navService: NavigationService,
    private _navRepo: NavigationRepository
  ) {}

  ngOnInit(): void {
    this._navService.loadUserNavigation().subscribe();
    this._navRepo.navData$
      .pipe(
        tap((navData) => {
          if (navData != null) {
            const defaultData = navData.find((x) => x.isDefault)?.navItems;
            const data = defaultData != null ? this.sortItems(defaultData) : [];
            this.dataSource.data = [...data];
          }
        }),
        takeUntil(this._destroy$)
      )
      .subscribe();
  }

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }

  hasChild(a: number, node: INavigationItem): boolean {
    return !!node.children && node.children.length > 0;
  }

  onRouterLinkActive(active: boolean, node: INavigationItem): void {
    active ? this.treeControl.expandDescendants(node) : void 0;
  }

  sortItems(items: INavigationItem[]): INavigationItem[] {
    items.forEach((element) => {
      if (element.children != null) {
        element.children = this.sortItems(element.children);
      }
    });
    return items.sort((a, b) => a.order - b.order);
  }
}
