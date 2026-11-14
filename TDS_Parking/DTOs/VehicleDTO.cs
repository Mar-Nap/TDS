using TDS_Parking.Enums;

namespace TDS_Parking.DTOs
{
    public class VehicleDTO
    {
        public string VehicleReg { get; init; } = "";
        public VehicleType VehicleType { get; init; }
    }
}
