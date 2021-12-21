import { MatFormFieldModule } from '@angular/material/form-field';
import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { LoadingComponent } from './components/loading/loading.component';
import { AirDateDirective } from './directives/air-date.directive';
import { AirSelectorComponent } from './components/air-selector/air-selector.component';
import { AirGroupedSelectorComponent } from './components/air-grouped-selector/air-grouped-selector.component';
import { MatSelectModule } from '@angular/material/select';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import {OverlayModule} from '@angular/cdk/overlay';
@NgModule({
  declarations: [
    LoadingComponent,
    AirDateDirective,
    AirSelectorComponent,
    AirGroupedSelectorComponent,
  ],
  imports: [
    MatProgressSpinnerModule,
    MatSelectModule,
    NgxMatSelectSearchModule,
    FormsModule,
    OverlayModule,
    MatInputModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    CommonModule,
    MatSnackBarModule,
    MatButtonModule,
  ],
  exports: [LoadingComponent, AirDateDirective, AirGroupedSelectorComponent],
  bootstrap: [AirGroupedSelectorComponent]
})
export class SharedModule {}
