import { Component, inject, signal } from '@angular/core';
import { form, FormField, maxLength, minLength, required } from '@angular/forms/signals';
import { VehicleExitForm } from '../../models';
import { Parking } from '../../services/parking';
import { ToastService } from 'ngx-toaster-next';

@Component({
  imports: [FormField],
  selector: 'app-parking-exit',
  templateUrl: './parking-exit.html',
})
export class ParkingExit {
  private parkingService = inject(Parking);
  private toasts = inject(ToastService);

  private vehicle = signal<VehicleExitForm>({
    vehicleReg: '',
  });

  public vehicleExitLoading = this.parkingService.parkingExitLoading;

  public vehicleExitForm = form(this.vehicle, (fieldPath) => {
    required(fieldPath.vehicleReg, { message: 'Vehicle registration is required' });
    minLength(fieldPath.vehicleReg, 4, {
      message: 'Vehicle registration must be at least 4 characters long',
    });
    maxLength(fieldPath.vehicleReg, 12, {
      message: 'Vehicle registration must be at most 12 characters long',
    });
  });

  public submitForm(event: Event) {
    event.preventDefault();
    if (this.vehicleExitForm().valid()) {
      this.parkingService.exitParking(this.vehicle().vehicleReg).subscribe((entry) => {
        this.toasts.success(
          `Vehicle ${entry.vehicleReg} exited parking successfully at ${entry.timeOut}. Total charge: ${entry.vehicleCharge}`,
        );
        this.vehicle.set({ vehicleReg: '' });
        this.vehicleExitForm().reset();
      });
    }
  }
}
