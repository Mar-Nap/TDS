namespace TDS_Parking.DTOs
{
    public class ParkingChargeDTO

    {
        public string VehicleReg { get; set; } = "";
        public decimal VehicleCharge { get; set; }
        public DateTimeOffset TimeIn { get; set; }
        public DateTimeOffset TimeOut { get; set; }
    }
}
