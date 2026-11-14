using TDS_Parking.Enums;
using TDS_Parking.Interfaces;
using TDS_Parking.Models;
using TDS_Parking.Services;

namespace TDS_Parking.Tests.Services;

public class ParkingServiceTests
{
    [Fact]
    public void GetParkingSlots_ReturnsRepositoryCounts()
    {
        var repository = new FakeParkingRepository
        {
            AvailableSpaces = 5,
            ParkedCars = 3
        };
        var service = new ParkingService(repository, new FixedTimeProvider(DateTimeOffset.UnixEpoch));

        var result = service.GetParkingSlots();

        Assert.Equal(5, result.AvailableSpaces);
        Assert.Equal(3, result.OccupiedSpaces);
    }

    [Fact]
    public void EnterParking_AddsEntryAndReturnsAssignedSlotAndEntryTime()
    {
        var now = new DateTimeOffset(2026, 11, 14, 12, 0, 0, TimeSpan.Zero);
        var repository = new FakeParkingRepository { AvailableSlotId = 4 };
        var service = new ParkingService(repository, new FixedTimeProvider(now));

        var result = service.EnterParking("ABC123", (int)VehicleType.MediumCar);

        Assert.Equal("ABC123", result.VehicleReg);
        Assert.Equal(4, result.SpaceNumber);
        Assert.Equal(now, result.TimeIn);
        Assert.NotNull(repository.AddedEntry);
        Assert.Equal(VehicleType.MediumCar, repository.AddedEntry.VehicleType);
        Assert.Equal(4, repository.AssignedSlotId);
        Assert.Equal(repository.AddedEntry.ParkingEntryId, repository.AssignedEntryId);
    }

    [Fact]
    public void ExitParking_CalculatesChargeForEachStartedMinuteAndFreesSlot()
    {
        var entryTime = new DateTimeOffset(2026, 11, 14, 12, 0, 30, TimeSpan.Zero);
        var exitTime = new DateTimeOffset(2026, 11, 14, 12, 3, 0, TimeSpan.Zero);
        var entry = new ParkingEntry
        {
            ParkingEntryId = 7,
            ParkingSlotId = 2,
            VehicleReg = "ABC123",
            VehicleType = VehicleType.MediumCar,
            TimeIn = entryTime
        };
        var repository = new FakeParkingRepository { EntryToReturn = entry };
        var service = new ParkingService(repository, new FixedTimeProvider(exitTime));

        var result = service.ExitParking("ABC123");

        Assert.Equal("ABC123", result.VehicleReg);
        Assert.Equal(entryTime, result.TimeIn);
        Assert.Equal(exitTime, result.TimeOut);
        Assert.Equal(1.1m, result.VehicleCharge);
        Assert.Same(entry, repository.UpdatedEntry);
        Assert.Equal(2, repository.RemovedSlotId);
    }

    [Fact]
    public void ExitParking_WhenNoActiveEntryExists_ThrowsAndDoesNotReleaseSlot()
    {
        var repository = new FakeParkingRepository();
        var service = new ParkingService(repository, new FixedTimeProvider(DateTimeOffset.UnixEpoch));

        Assert.Throws<KeyNotFoundException>(() => service.ExitParking("UNKNOWN"));

        Assert.Null(repository.UpdatedEntry);
        Assert.Null(repository.RemovedSlotId);
    }

    private sealed class FixedTimeProvider(DateTimeOffset utcNow) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => utcNow;
    }

    private sealed class FakeParkingRepository : IParkingRepository
    {
        public int AvailableSpaces { get; init; }
        public int ParkedCars { get; init; }
        public int AvailableSlotId { get; init; } = 1;
        public ParkingEntry? AddedEntry { get; private set; }
        public ParkingEntry? EntryToReturn { get; init; }
        public ParkingEntry? UpdatedEntry { get; private set; }
        public int? AssignedEntryId { get; private set; }
        public int? AssignedSlotId { get; private set; }
        public int? RemovedSlotId { get; private set; }

        public int GetParkedCarsCount() => ParkedCars;
        public int GetAvailableSpacesCount() => AvailableSpaces;
        public int GetAvailableSlotId() => AvailableSlotId;

        public void AddParkingEntry(ParkingEntry entry)
        {
            entry.ParkingEntryId = 1;
            AddedEntry = entry;
        }

        public AdditionalCharge GetAdditionalCharge() => new AdditionalCharge { Rate = 0.5m, Period = 2 };

        public void UpdateParkingEntry(ParkingEntry entry) => UpdatedEntry = entry;
        public ParkingEntry? GetParkingEntryByVehicleReg(string vehicleReg) => EntryToReturn;
        public void AssignParkingEntryToSlot(int parkingEntryId, int parkingSlotId)
        {
            AssignedEntryId = parkingEntryId;
            AssignedSlotId = parkingSlotId;
        }

        public void RemoveParkingEntryFromSlot(int parkingSlotId) => RemovedSlotId = parkingSlotId;

        public decimal GetParkingCharge(VehicleType type) => type switch
        {
            VehicleType.SmallCar => 0.1m,
            VehicleType.MediumCar => 0.2m,
            VehicleType.LargeCar => 0.4m,
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };
    }
}
