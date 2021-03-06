import { DragDropModule } from '@angular/cdk/drag-drop';
import { OverlayModule } from '@angular/cdk/overlay';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDividerModule } from '@angular/material/divider';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatTreeModule } from '@angular/material/tree';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
// import { ErrorSaveCategoryTreeComponent } from './components/error-save-category-tree/error-save-category-tree.component';
import { TreeItemEditComponent } from './components/editable-tree/edit-item/tree-item-edit.component';
import { TreeItemEditOrderComponent } from './components/editable-tree/edit-order/tree-item-order.component';
import { EditableTreeComponent } from './components/editable-tree/editable-tree.component';
import { GroupedSelectorComponent } from './components/grouped-selector/grouped-selector.component';
import { LoadingComponent } from './components/loading/loading.component';
import { SnackbarService } from './services/snackbar.service';
@NgModule({
  declarations: [
    LoadingComponent,
    GroupedSelectorComponent,

    TreeItemEditComponent,
    TreeItemEditOrderComponent,
    EditableTreeComponent,
    // ErrorSaveCategoryTreeComponent
  ],
  imports: [
    MatProgressSpinnerModule,
    MatSelectModule,
    NgxMatSelectSearchModule,
    FormsModule,
    OverlayModule,
    MatInputModule,
    DragDropModule,
    MatTreeModule,
    MatTooltipModule,
    MatCheckboxModule,
    MatDividerModule,
    MatIconModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    CommonModule,
    MatSnackBarModule,
    MatButtonModule,
  ],
  providers: [SnackbarService],
  exports: [LoadingComponent, GroupedSelectorComponent, EditableTreeComponent],
  bootstrap: [GroupedSelectorComponent],
})
export class SharedModule {}
