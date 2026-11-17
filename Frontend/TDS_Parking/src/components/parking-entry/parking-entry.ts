import { Component, inject, signal } from '@angular/core';
import { ToastService } from 'ngx-toaster-next';
import { VehicleEntryForm } from '../../models';
import { form, FormField, maxLength, minLength, required } from '@angular/forms/signals';
import { Parking } from '../../services/parking';
import { VehicleType } from '../../enums/vehicle-type.enum';

@Component({
  imports: [FormField],
  selector: 'app-parking-entry',
  templateUrl: './parking-entry.html',
})
export class ParkingEntry {
  private readonly parkingService = inject(Parking);
  private readonly toasts = inject(ToastService);

  private vehicle = signal<VehicleEntryForm>({
    vehicleReg: '',
    vehicleType: '',
  });

  public vehicleEntryLoading = this.parkingService.parkingEntryLoading;

  public vehicleEntryForm = form(this.vehicle, (fieldPath) => {
    required(fieldPath.vehicleReg, { message: 'Vehicle registration is required' });
    minLength(fieldPath.vehicleReg, 4, {
      message: 'Vehicle registration must be at least 4 characters long',
    });
    maxLength(fieldPath.vehicleReg, 12, {
      message: 'Vehicle registration must be at most 12 characters long',
    });
    required(fieldPath.vehicleType, { message: 'Vehicle type is required' });
  });

  public submitForm(event: Event) {
    event.preventDefault();
    if (this.vehicleEntryForm().valid()) {
      const vehicleType = VehicleType[this.vehicle().vehicleType as keyof typeof VehicleType];
      this.parkingService
        .enterParking(this.vehicle().vehicleReg, vehicleType)
        .subscribe((entry) => {
          this.toasts.success(
            `Vehicle ${entry.vehicleReg} entered parking successfully at ${entry.timeIn}. Please move to space ${entry.spaceNumber}`,
          );
          this.vehicle.set({ vehicleReg: '', vehicleType: '' });
          this.vehicleEntryForm().reset();
        });
    }
  }
}
