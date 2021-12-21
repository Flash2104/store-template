import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';
import { RouterModule, Routes } from '@angular/router';
import { SharedModule } from '../../../shared/shared.module';
import { TeamCreateComponent } from './create/team-create.component';
import { TeamCreateRepository } from './repository/team-create.repository';
import { TeamCreateService } from './repository/team-create.service';

const routes: Routes = [
  {
    path: '',
    children: [
      { path: '*', redirectTo: '/', pathMatch: 'full' },
      {
        path: '',
        component: TeamCreateComponent,
      },
    ],
  },
];

@NgModule({
  declarations: [TeamCreateComponent],
  imports: [
    MatButtonModule,
    MatCardModule,
    MatDatepickerModule,
    MatTooltipModule,
    MatNativeDateModule,
    MatIconModule,
    CommonModule,
    MatTableModule,
    MatInputModule,
    FormsModule,
    MatDividerModule,
    ReactiveFormsModule,
    SharedModule,
    RouterModule.forChild(routes),
  ],
  providers: [TeamCreateService, TeamCreateRepository],
})
export class TeamCreateModule {}
