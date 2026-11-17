import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ParkingEntry } from './parking-entry';
import { provideHttpClientTesting } from '@angular/common/http/testing';

describe('ParkingEntry', () => {
  let component: ParkingEntry;
  let fixture: ComponentFixture<ParkingEntry>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      providers: [provideHttpClientTesting()],
      imports: [ParkingEntry],
    }).compileComponents();

    fixture = TestBed.createComponent(ParkingEntry);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
