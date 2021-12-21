import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatTreeModule } from '@angular/material/tree';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from '../shared/shared.module';
import { SideNavContainerComponent } from './sidenav-container.component';

const routes: Routes = [
  {
    path: '',
    component: SideNavContainerComponent,
    children: [
      { path: '', redirectTo: 'profile', pathMatch: 'full' },
      { path: '*', redirectTo: 'profile', pathMatch: 'full' },
      {
        path: 'team',
        loadChildren: () =>
          import('../private/team/team.module').then((m) => m.TeamModule),
      },
      {
        path: 'profile',
        loadChildren: () =>
          import('../private/profile/profile.module').then(
            (m) => m.ProfileModule
          ),
      },
    ],
  },
];

@NgModule({
  declarations: [SideNavContainerComponent],
  imports: [
    MatButtonModule,
    MatSidenavModule,
    MatGridListModule,
    MatIconModule,
    MatTreeModule,
    SharedModule,
    RouterModule.forChild(routes),
  ],
})
export class PrivateModule {}
