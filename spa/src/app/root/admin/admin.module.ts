import { DragDropModule } from '@angular/cdk/drag-drop';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDividerModule } from '@angular/material/divider';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from 'src/app/shared/shared.module';
import { AdminNavigationComponent } from './admin-navigation.component';
import { AdminCategoryTreesComponent } from './categories/admin-category-trees.component';
import { AdminProductsComponent } from './products/admin-products.component';
import { AdminShopComponent } from './shop/admin-shop.component';

const routes: Routes = [
  {
    path: '',
    component: AdminNavigationComponent,
    children: [
      { path: '', redirectTo: 'shop', pathMatch: 'full' },
      { path: '*', redirectTo: 'shop', pathMatch: 'full' },
      {
        path: 'shop',
        component: AdminShopComponent,
      },
      {
        path: 'categories',
        component: AdminCategoryTreesComponent,
      },
      {
        path: 'products',
        component: AdminProductsComponent,
      },
    ],
  },
];

@NgModule({
  declarations: [
    AdminNavigationComponent,
    AdminShopComponent,
    AdminCategoryTreesComponent,
    AdminProductsComponent,
  ],
  imports: [
    MatButtonModule,
    MatSidenavModule,
    MatGridListModule,
    MatCardModule,
    MatIconModule,
    DragDropModule,
    MatSelectModule,
    MatTooltipModule,
    MatTableModule,
    MatDividerModule,
    MatCheckboxModule,
    MatInputModule,
    CommonModule,
    ReactiveFormsModule,
    SharedModule,
    RouterModule.forChild(routes),
  ],
})
export class AdminModule {}
