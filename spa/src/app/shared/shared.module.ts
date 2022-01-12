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
import { MatTreeModule } from '@angular/material/tree';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { TreeItemEditComponent } from './components/editable-tree/edit-item/tree-item-edit.component';
import { TreeItemEditOrderComponent } from './components/editable-tree/edit-order/tree-item-order.component';
import { EditableTreeComponent } from './components/editable-tree/editable-tree.component';
import { GroupedSelectorComponent } from './components/grouped-selector/grouped-selector.component';
import { LoadingComponent } from './components/loading/loading.component';
@NgModule({
  declarations: [
    LoadingComponent,
    GroupedSelectorComponent,

    TreeItemEditComponent,
    TreeItemEditOrderComponent,
    EditableTreeComponent,
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
    MatCheckboxModule,
    MatDividerModule,
    MatIconModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    CommonModule,
    MatSnackBarModule,
    MatButtonModule,
  ],
  exports: [LoadingComponent, GroupedSelectorComponent, EditableTreeComponent],
  bootstrap: [GroupedSelectorComponent],
})
export class SharedModule {}
