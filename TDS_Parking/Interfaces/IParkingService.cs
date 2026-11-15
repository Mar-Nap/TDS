using TDS_Parking.DTOs;
using TDS_Parking.Enums;

namespace TDS_Parking.Interfaces
{
    public interface IParkingService
    {
        public ParkingEntryDTO EnterParking(string vehicleRegistrationNumber, VehicleType vehicleType);
        public ParkingStatusDTO GetParkingSlots();


        public ParkingChargeDTO ExitParking(string vehicleRegistrationNumber);
    }
}
