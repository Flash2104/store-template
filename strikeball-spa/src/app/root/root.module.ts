import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PrivateGuard } from '../shared/guards/private.guard';

const routes: Routes = [
  {
    path: '',
    children: [
      { path: '', redirectTo: 'private', pathMatch: 'full' },
      { path: '*', redirectTo: 'private', pathMatch: 'full' },
      {
        path: 'public',
        loadChildren: () =>
          import('../public/public.module').then((m) => m.PublicModule),
        data: {
          animation: 'PublicPages',
        },
      },
      {
        path: 'private',
        loadChildren: () =>
          import('../private/private.module').then((m) => m.PrivateModule),
        canActivate: [PrivateGuard],
        data: {
          animation: 'PrivatePages',
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
