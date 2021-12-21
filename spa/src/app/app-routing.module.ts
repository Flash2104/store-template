import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RootModule } from './root/root.module';

const routes: Routes = [
  {
    path: '',
    loadChildren: () => RootModule,
  },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      onSameUrlNavigation: 'reload',
      enableTracing: false,
    }),
  ],
  exports: [RouterModule],
})
export class AppRoutingModule {}
