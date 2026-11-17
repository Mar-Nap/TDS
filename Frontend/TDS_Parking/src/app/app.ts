import { Component, signal, inject } from '@angular/core';
import { Parking } from '../services/parking';
import { ParkingCharge } from '../models';
import { ParkingStatusBar } from '../components/parking-status/parking-status';
import { RouterOutlet } from '@angular/router';


@Component({
  imports: [ParkingStatusBar, RouterOutlet],
  selector: 'app-root',
  styleUrl: './app.scss',
  templateUrl: './app.html',
})
export class App {
  protected readonly title = signal('TDS_Parking');
  private readonly parkingService = inject(Parking);

  protected readonly parkingStatusLoading = this.parkingService.parkingStatusLoading;
  protected readonly parkingStatusError = this.parkingService.parkingStatusError;

  protected readonly parkingEntryLoading = this.parkingService.parkingEntryLoading;
  protected readonly parkingEntryError = this.parkingService.parkingEntryError;

  protected readonly parkingExitLoading = this.parkingService.parkingExitLoading;
  protected readonly parkingExitError = this.parkingService.parkingExitError;

  public parkingCharge: ParkingCharge = {
    vehicleReg: '',
    vehicleCharge: 0,
    timeIn: '',
    timeOut: '',
  };

  public parkingStatusSignal = this.parkingService.parkingStatus;
}
