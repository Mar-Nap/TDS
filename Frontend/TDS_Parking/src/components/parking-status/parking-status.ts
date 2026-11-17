import { Component, input } from '@angular/core';

@Component({
  imports: [],
  selector: 'app-parking-status',
  styleUrl: './parking-status.scss',
  templateUrl: './parking-status.html',
})
export class ParkingStatusBar {
  parkedCars = input(0);
  emptySlots = input(0);
}
