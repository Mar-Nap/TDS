
using TDS_Parking.Enums;
using TDS_Parking.Exceptions;
using TDS_Parking.Interfaces;
using TDS_Parking.Models;

namespace TDS_Parking.Repository
{
    public class ParkingRepository : IParkingRepository
    {
        private List<ParkingSlot> _parkingSlots = new List<ParkingSlot>()
        {
            new ParkingSlot { ParkingSlotId = 1, ParkingEntryId = null },
            new ParkingSlot { ParkingSlotId = 2, ParkingEntryId = null },
            new ParkingSlot { ParkingSlotId = 3, ParkingEntryId = null },
            new ParkingSlot { ParkingSlotId = 4, ParkingEntryId = null },
            new ParkingSlot { ParkingSlotId = 5, ParkingEntryId = null },
            new ParkingSlot { ParkingSlotId = 6, ParkingEntryId = null },
            new ParkingSlot { ParkingSlotId = 7, ParkingEntryId = null },
            new ParkingSlot { ParkingSlotId = 8, ParkingEntryId = null },

        };

        private Dictionary<VehicleType, decimal> _charges = new Dictionary<VehicleType, decimal>()
        {
            { VehicleType.SmallCar, 0.1m },
            { VehicleType.MediumCar, 0.2m },
            { VehicleType.LargeCar, 0.4m }
        };

        private readonly AdditionalCharge _additionalCharge = new AdditionalCharge()
        {
            Rate = 1,
            Period = 5
        };


        private List<ParkingEntry> _parkingEntries = new List<ParkingEntry>();

        public int GetParkedCarsCount()
        {
            return this._parkingSlots.Count(slot => slot.ParkingEntryId != null);
        }

        public AdditionalCharge GetAdditionalCharge()
        {
            return _additionalCharge;
        }

        public int GetAvailableSpacesCount()
        {
            return this._parkingSlots.Count(slot => slot.ParkingEntryId == null);
        }

        public int GetAvailableSlotId()
        {
            var availableSlot = this._parkingSlots.OrderBy(s => s.ParkingSlotId).FirstOrDefault(slot => slot.ParkingEntryId == null);
            if (availableSlot != null)
            {
                return availableSlot.ParkingSlotId;
            }
            else
            {
                throw new ApplicationException("No available parking slots.");
            }
        }

        public void AddParkingEntry(ParkingEntry entry)
        {
            var existingEntry = GetParkingEntryByVehicleReg(entry.VehicleReg);
            if (existingEntry != null)
            {
                throw new VehicleAlreadyParkedException(entry.VehicleReg);
            }
            ;
            var currentMaxId = this._parkingEntries.Any() ? this._parkingEntries.Max(e => e.ParkingEntryId) : 0;
            entry.ParkingEntryId = currentMaxId + 1;
            this._parkingEntries.Add(entry);
        }

        public ParkingEntry? GetParkingEntryByVehicleReg(string vehicleReg)
        {
            return this._parkingEntries.FirstOrDefault(e => String.Equals(e.VehicleReg, vehicleReg, StringComparison.OrdinalIgnoreCase) && e.TimeOut == null);
        }

        public void UpdateParkingEntry(ParkingEntry entry)
        {
            var existingEntry = this._parkingEntries.FirstOrDefault(e => e.ParkingEntryId == entry.ParkingEntryId);
            if (existingEntry != null)
            {
                existingEntry.VehicleReg = entry.VehicleReg;
                existingEntry.VehicleCharge = entry.VehicleCharge;
                existingEntry.VehicleType = entry.VehicleType;
                existingEntry.TimeIn = entry.TimeIn;
                existingEntry.TimeOut = entry.TimeOut;
            }
            else
            {
                throw new ApplicationException("Parking entry not found.");
            }
        }

        public void AssignParkingEntryToSlot(int parkingEntryId, int parkingSlotId)
        {
            var slot = GetSlotById(parkingSlotId);
            slot.ParkingEntryId = parkingEntryId;

        }

        public void RemoveParkingEntryFromSlot(int parkingSlotId)
        {
            var slot = GetSlotById(parkingSlotId);
            slot.ParkingEntryId = null;

        }

        public decimal GetParkingCharge(VehicleType type)
        {
            return this._charges[type];
        }

        private ParkingSlot GetSlotById(int parkingSlotId)
        {
            var slot = this._parkingSlots.FirstOrDefault(s => s.ParkingSlotId == parkingSlotId);
            if (slot == null)
            {
                throw new ApplicationException("Parking slot not found.");
            }
            return slot;
        }

    }
}
