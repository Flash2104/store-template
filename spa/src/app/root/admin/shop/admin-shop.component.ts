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
  selector: 'str-admin-shop',
  templateUrl: './admin-shop.component.html',
  styleUrls: ['./admin-shop.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AdminShopComponent implements OnInit, OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();


  ngOnInit(): void {
  }

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }
}
