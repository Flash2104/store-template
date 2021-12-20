import { IGroupedReferenceData } from './../../../../shared/services/dto-models/reference-data';
import { IRegionData } from './../../../../shared/services/dto-models/references/cities/cities-dto';
import { CitiesService } from './../../../../shared/services/references/cities.service';
import { Location } from '@angular/common';
import {
  ChangeDetectionStrategy,
  Component,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { debounceTime, map, Observable, Subject, takeUntil, tap } from 'rxjs';
import { CitiesRepository } from 'src/app/shared/repository/cities.repository';
import { TeamCreateRepository } from '../repository/team-create.repository';
import { TeamCreateService } from '../repository/team-create.service';
import { MatSelect } from '@angular/material/select';

@Component({
  selector: 'air-team-create',
  templateUrl: './team-create.component.html',
  styleUrls: ['./team-create.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [CitiesRepository, CitiesService],
})
export class TeamCreateComponent implements OnInit, OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  public loading$: Observable<boolean> = this._teamCreateRepo.loading$.pipe(
    takeUntil(this._destroy$)
  );

  form: FormGroup = new FormGroup({
    title: new FormControl(null, Validators.required),
    city: new FormControl(null),
    foundationDate: new FormControl(null),
  });

  public cities$: Observable<IGroupedReferenceData<number>[]> =
    this._citiesRepo.cities$.pipe(
      map((regions): IGroupedReferenceData<number>[] => {
        if (regions == null) {
          return [];
        }
        return regions.map((region) => {
          return {
            key: region.title,
            data:
              region.cities?.map((city) => {
                return {
                  id: city.id,
                  title: city.city,
                };
              }) || [],
          } as IGroupedReferenceData<number>;
        });
      }),
      takeUntil(this._destroy$)
    );

  public loadingCities$: Observable<boolean> = this._citiesRepo.loading$.pipe(
    takeUntil(this._destroy$)
  );

  constructor(
    private _teamCreateRepo: TeamCreateRepository,
    private _teamCreateService: TeamCreateService,
    private _location: Location,
    private _sanitizer: DomSanitizer,
    private _citiesRepo: CitiesRepository,
    private _citiesService: CitiesService
  ) {}

  // eslint-disable-next-line @angular-eslint/no-empty-lifecycle-method
  ngOnInit(): void {
    this._citiesService.loadCityReferences().subscribe();
    this._teamCreateRepo.createData$
      .pipe(
        tap((data) => {
          if (data == null) {
            this.form.reset();
          } else {
            this.form.setValue(data, { emitEvent: false });
          }
        }),
        takeUntil(this._destroy$)
      )
      .subscribe();
    this.form.valueChanges
      .pipe(
        debounceTime(400),
        tap((data) => {
          this._teamCreateRepo.patchCreateData(data);
        }),
        takeUntil(this._destroy$)
      )
      .subscribe();
  }

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }

  onCancel(): void {
    this._location.back();
  }

  onSave(): void {
    this._teamCreateService.createTeam().subscribe();
  }
}
