namespace TDS_Parking.DTOs
{
    public class ParkingEntryDTO
    {
        public string VehicleReg { get; set; } = "";
        public int SpaceNumber { get; set; }
        public DateTimeOffset TimeIn { get; set; }
    }
}
