namespace TDS_Parking.Exceptions
{
    public class VehicleAlreadyParkedException : Exception
    {
        public VehicleAlreadyParkedException(string vehicleReg)
            : base($"Vehicle '{vehicleReg}' already has an active parking session.")
        {
        }
    }
}
