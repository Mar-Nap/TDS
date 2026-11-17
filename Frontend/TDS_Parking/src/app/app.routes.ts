
import { Routes } from '@angular/router';
import { ParkingEntry } from '../components/parking-entry/parking-entry';
import { ParkingExit } from '../components/parking-exit/parking-exit';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'enter',
    pathMatch: 'full',
  },
  {
    path: 'enter',
    component: ParkingEntry,
  },
  {
    path: 'exit',
    component: ParkingExit,
  },
];
