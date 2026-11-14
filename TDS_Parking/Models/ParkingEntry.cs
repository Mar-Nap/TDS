using TDS_Parking.Enums;

namespace TDS_Parking.Models
{
    public class ParkingEntry
    {
        public int ParkingEntryId { get; set; }
        public int ParkingSlotId { get; set; }
        public string VehicleReg { get; set; } = "";
        public decimal VehicleCharge { get; set; }
        public VehicleType VehicleType { get; set; }
        public DateTimeOffset TimeIn { get; set; }
        public DateTimeOffset? TimeOut { get; set; }

    }
}
