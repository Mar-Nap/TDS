using TDS_Parking.DTOs;
using TDS_Parking.Enums;
using TDS_Parking.Interfaces;
using TDS_Parking.Models;

namespace TDS_Parking.Services
{
    public class ParkingService : IParkingService
    {
        private readonly IParkingRepository repository;
        private readonly TimeProvider timeProvider;

        public ParkingService(IParkingRepository repo, TimeProvider timeProvider)
        {
            repository = repo;
            this.timeProvider = timeProvider;
        }

        public ParkingStatusDTO GetParkingSlots()
        {
            return new ParkingStatusDTO
            {
                AvailableSpaces = repository.GetAvailableSpacesCount(),
                OccupiedSpaces = repository.GetParkedCarsCount()
            };
        }

        public ParkingEntryDTO EnterParking(string vehicleRegistrationNumber, int vehicleType)
        {
            var availableSlot = repository.GetAvailableSlotId();
            var parkingEntry = new ParkingEntry()
            {
                ParkingSlotId = availableSlot,
                VehicleReg = vehicleRegistrationNumber,
                VehicleType = (Enums.VehicleType)vehicleType,
                TimeIn = timeProvider.GetUtcNow()
            };
            repository.AddParkingEntry(parkingEntry);
            repository.AssignParkingEntryToSlot(parkingEntry.ParkingEntryId, availableSlot);

            return new ParkingEntryDTO
            {
                VehicleReg = parkingEntry.VehicleReg,
                SpaceNumber = parkingEntry.ParkingSlotId,
                TimeIn = parkingEntry.TimeIn
            };
        }

        public ParkingChargeDTO ExitParking(string vehicleRegistrationNumber)
        {
            var parkingEntry = repository.GetParkingEntryByVehicleReg(vehicleRegistrationNumber);

            if (parkingEntry is null)
                throw new KeyNotFoundException("Parking entry not found.");

            var currentTime = timeProvider.GetUtcNow();

            var timeSpan = (currentTime - parkingEntry.TimeIn).TotalMinutes;

            var additionalCharge = repository.GetAdditionalCharge();

            var parkingChargePerMinute = CalculatePerMinuteCharge(parkingEntry.VehicleType, timeSpan);
            var parkingChargePerPeriods = CalculatePerPeriodCharge(parkingEntry.VehicleType, timeSpan, additionalCharge);

            parkingEntry.TimeOut = currentTime;
            parkingEntry.VehicleCharge = parkingChargePerMinute + parkingChargePerPeriods;

            repository.UpdateParkingEntry(parkingEntry);
            repository.RemoveParkingEntryFromSlot(parkingEntry.ParkingSlotId);

            return new ParkingChargeDTO()
            {
                VehicleReg = parkingEntry.VehicleReg,
                TimeIn = parkingEntry.TimeIn,
                TimeOut = parkingEntry.TimeOut.Value,
                VehicleCharge = parkingEntry.VehicleCharge
            };

        }

        private decimal CalculatePerMinuteCharge(VehicleType vehicleType, double timeSpan) =>
            repository.GetParkingCharge(vehicleType) * ((int)Math.Ceiling(timeSpan));

        private decimal CalculatePerPeriodCharge(VehicleType vehicleType, double timeSpan, AdditionalCharge additionalCharge) =>
            (decimal)Math.Floor((double)timeSpan / additionalCharge.Period) * additionalCharge.Rate;
    }


}
