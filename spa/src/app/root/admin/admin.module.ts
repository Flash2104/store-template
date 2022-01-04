import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatTreeModule } from '@angular/material/tree';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from 'src/app/shared/shared.module';
import { AdminNavigationComponent } from './admin-navigation.component';
import { AdminCategoriesComponent } from './categories/admin-categories.component';
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
        component: AdminCategoriesComponent,
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
    AdminCategoriesComponent,
    AdminProductsComponent,
  ],
  imports: [
    MatButtonModule,
    MatSidenavModule,
    MatGridListModule,
    MatCardModule,
    MatIconModule,
    MatTreeModule,
    SharedModule,
    RouterModule.forChild(routes),
  ],
})
export class AdminModule {}
