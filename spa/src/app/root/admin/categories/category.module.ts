import { MatSnackBarModule } from '@angular/material/snack-bar';
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
import { AdminCategoryTreesComponent } from './components/admin-category-trees.component';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { CategoryService } from './repository/category.service';
import { ErrorSaveCategoryTreeComponent } from './components/error-save-category-tree/error-save-category-tree.component';
import { SnackbarService } from 'src/app/shared/services/snackbar.service';
import { CategoryRepository } from './repository/category.repository';

const routes: Routes = [
  {
    path: '',
    component: AdminCategoryTreesComponent,
  },
];

@NgModule({
  declarations: [ErrorSaveCategoryTreeComponent, AdminCategoryTreesComponent],
  imports: [
    MatButtonModule,
    MatSidenavModule,
    MatGridListModule,
    MatSnackBarModule,
    MatCardModule,
    MatIconModule,
    DragDropModule,
    MatProgressBarModule,
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
  providers: [CategoryService, CategoryRepository, SnackbarService],
  bootstrap: [AdminCategoryTreesComponent],
})
export class CategoryModule {}
