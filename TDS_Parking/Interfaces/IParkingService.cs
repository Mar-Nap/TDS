using TDS_Parking.DTOs;

namespace TDS_Parking.Interfaces
{
    public interface IParkingService
    {
        public ParkingEntryDTO EnterParking(string vehicleRegistrationNumber, int vehicleType);
        public ParkingStatusDTO GetParkingSlots();


        public ParkingChargeDTO ExitParking(string vehicleRegistrationNumber);
    }
}
