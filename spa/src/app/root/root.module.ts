import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminPagesGuard } from '../shared/guards/private.guard';

const routes: Routes = [
  {
    path: '',
    children: [
      { path: '', redirectTo: 'shop', pathMatch: 'full' },
      { path: '*', redirectTo: 'shop', pathMatch: 'full' },
      {
        path: 'shop',
        loadChildren: () =>
          import('../shop/shop.module').then((m) => m.StoreModule),
        data: {
          animation: 'ShopPages',
        },
      },
      {
        path: 'admin',
        loadChildren: () =>
          import('../admin/admin.module').then((m) => m.PrivateModule),
        canActivate: [AdminPagesGuard],
        data: {
          animation: 'AdminPages',
        },
      },
    ],
  },
];

@NgModule({
  declarations: [],
  imports: [RouterModule.forChild(routes)],
})
export class RootModule {}
