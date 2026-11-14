using Microsoft.AspNetCore.Mvc;
using TDS_Parking.DTOs;
using TDS_Parking.Interfaces;

namespace TDS_Parking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ParkingController : ControllerBase
    {
        private IParkingService parkingService;
        public ParkingController(IParkingService parkingService)
        {
            this.parkingService = parkingService;
        }

        [HttpGet]
        public ActionResult<ParkingStatusDTO> getParkingSpaces()
        {
            return Ok(parkingService.GetParkingSlots());
        }

        [HttpPost]
        public ActionResult<ParkingEntryDTO> enterParking([FromBody] VehicleDTO vehicle)
        {
            return Ok(parkingService.EnterParking(vehicle.VehicleReg, (int)vehicle.VehicleType));
        }

        [HttpPost("exit")]
        public ActionResult<ParkingChargeDTO> exitParking([FromBody] VehicleDTO vehicle)
        {
            return Ok(parkingService.ExitParking(vehicle.VehicleReg));
        }
    }
}
