import { inject, Service, signal } from '@angular/core';
import { environment } from '../environments/environment';
import { HttpClient } from '@angular/common/http';
import { map, Observable, tap } from 'rxjs';
import { VehicleType } from '../enums/vehicle-type.enum';
import { ParkingCharge, ParkingEntry, ParkingStatus } from '../models';
import { ToastService } from 'ngx-toaster-next';

@Service()
export class Parking {
  private httpClient = inject(HttpClient);
  private toasts = inject(ToastService);
  private apiUrl = environment.apiUrl;

  constructor() {
    this.getParkingStatus();
  }

  private readonly _parkingStatusLoading = signal(false);
  public readonly parkingStatusLoading = this._parkingStatusLoading.asReadonly();

  private readonly _parkingStatusError = signal(false);
  public readonly parkingStatusError = this._parkingStatusError.asReadonly();

  private readonly _parkingEntryLoading = signal(false);
  public readonly parkingEntryLoading = this._parkingEntryLoading.asReadonly();

  private readonly _parkingEntryError = signal(false);
  public readonly parkingEntryError = this._parkingEntryError.asReadonly();

  private readonly _parkingExitLoading = signal(false);
  public readonly parkingExitLoading = this._parkingExitLoading.asReadonly();

  private readonly _parkingExitError = signal(false);
  public readonly parkingExitError = this._parkingExitError.asReadonly();

  private readonly _parkingStatus = signal<ParkingStatus>({
    availableSpaces: 0,
    occupiedSpaces: 0,
  });
  public readonly parkingStatus = this._parkingStatus.asReadonly();

  private getParkingStatus(): void {
    this._parkingStatusLoading.set(true);
    this.httpClient
      .get<ParkingStatus>(`${this.apiUrl}/parking`)
      .pipe(
        tap({
          next: () => {
            this._parkingStatusLoading.set(false);
            this._parkingStatusError.set(false);
          },
          error: (error) => {
            this.toasts.error(`Error fetching parking status: ${error.error.detail}`);
            this._parkingStatusLoading.set(false);
            this._parkingStatusError.set(true);
          },
        }),
      )
      .subscribe((status) => {
        this._parkingStatus.set(status);
      });
  }

  public enterParking(registration: string, vehicleType: VehicleType): Observable<ParkingEntry> {
    this._parkingEntryLoading.set(true);
    return this.httpClient
      .post<ParkingEntry>(`${this.apiUrl}/parking`, {
        VehicleReg: registration,
        VehicleType: vehicleType,
      })
      .pipe(
        tap({
          next: () => {
            this.getParkingStatus();
            this._parkingEntryLoading.set(false);
            this._parkingEntryError.set(false);
          },
          error: (error) => {
            this.toasts.error(`Error entering parking: ${error.error.detail}`);
            this._parkingEntryLoading.set(false);
            this._parkingEntryError.set(true);
          },
        }),
        map((entry) => {
          entry.timeIn = new Date(entry.timeIn).toLocaleTimeString();
          return entry;
        }),
      );
  }

  public exitParking(registration: string): Observable<ParkingCharge> {
    this._parkingExitLoading.set(true);
    return this.httpClient
      .post<ParkingCharge>(`${this.apiUrl}/parking/exit`, {
        VehicleReg: registration,
      })
      .pipe(
        tap({
          next: () => {
            this.getParkingStatus();
            this._parkingExitLoading.set(false);
            this._parkingExitError.set(false);
          },
          error: (error) => {
            this.toasts.error(`Error exiting parking: ${error.error.detail}`);
            this._parkingExitLoading.set(false);
            this._parkingExitError.set(true);
          },
        }),
        map((charge) => {
          charge.timeIn = new Date(charge.timeIn).toLocaleTimeString();
          charge.timeOut = new Date(charge.timeOut).toLocaleTimeString();
          return charge;
        }),
      );
  }
}
