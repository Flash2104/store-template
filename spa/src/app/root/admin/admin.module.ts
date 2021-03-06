import { MatDividerModule } from '@angular/material/divider';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatTooltipModule } from '@angular/material/tooltip';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from 'src/app/shared/shared.module';
import { AdminNavigationComponent } from './admin-navigation.component';
import { AdminProductsComponent } from './products/admin-products.component';
import { AdminShopComponent } from './shop/admin-shop.component';
import { SnackbarService } from 'src/app/shared/services/snackbar.service';

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
        loadChildren: () =>
          import('./categories/category.module').then((m) => m.CategoryModule)
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
    AdminProductsComponent,
  ],
  imports: [
    MatButtonModule,
    MatSidenavModule,
    MatGridListModule,
    MatCardModule,
    MatIconModule,
    MatTooltipModule,
    MatInputModule,
    MatDividerModule,
    CommonModule,
    ReactiveFormsModule,
    SharedModule,
    RouterModule.forChild(routes),
  ],
  providers: [SnackbarService]
})
export class AdminModule {}
