using TDS_Parking.Enums;
using TDS_Parking.Models;

namespace TDS_Parking.Interfaces
{
    public interface IParkingRepository
    {
        public int GetParkedCarsCount();
        public int GetAvailableSpacesCount();
        public int GetAvailableSlotId();
        public void AddParkingEntry(ParkingEntry entry);
        public void UpdateParkingEntry(ParkingEntry entry);
        public ParkingEntry? GetParkingEntryByVehicleReg(string vehicleReg);
        public void AssignParkingEntryToSlot(int parkingEntryId, int parkingSlotId);
        public void RemoveParkingEntryFromSlot(int parkingSlotId);

        public decimal GetParkingCharge(VehicleType type);
        public AdditionalCharge GetAdditionalCharge();
    }
}
