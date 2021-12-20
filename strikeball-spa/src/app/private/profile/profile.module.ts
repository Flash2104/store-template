import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatTableModule } from '@angular/material/table';
import { MatTreeModule } from '@angular/material/tree';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';
import { ProfileMainInfoEditComponent } from './edit/profile-main-info-edit.component';
import { ProfileMainPageComponent } from './profile-main-page.component';

const routes: Routes = [
  {
    path: '',
    children: [
      { path: '*', redirectTo: '/', pathMatch: 'full' },
      {
        path: '',
        component: ProfileMainPageComponent,
      },
      {
        path: ':id',
        component: ProfileMainPageComponent,
      },
      {
        path: 'edit',
        component: ProfileMainInfoEditComponent,
      },
    ],
  },
];

@NgModule({
  declarations: [ProfileMainPageComponent, ProfileMainInfoEditComponent],
  imports: [
    MatButtonModule,
    MatSidenavModule,
    MatGridListModule,
    MatCardModule,
    MatListModule,
    MatIconModule,
    CommonModule,
    MatTableModule,
    MatInputModule,
    FormsModule,
    MatDividerModule,
    MatTreeModule,
    ReactiveFormsModule,
    SharedModule,
    RouterModule.forChild(routes),
  ],
})
export class ProfileModule {}
