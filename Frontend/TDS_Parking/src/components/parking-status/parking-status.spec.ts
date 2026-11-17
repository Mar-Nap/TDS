import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ParkingStatusBar } from './parking-status';

describe('ParkingStatus', () => {
  let component: ParkingStatusBar;
  let fixture: ComponentFixture<ParkingStatusBar>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ParkingStatusBar],
    }).compileComponents();

    fixture = TestBed.createComponent(ParkingStatusBar);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
