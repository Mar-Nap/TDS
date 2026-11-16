using Microsoft.AspNetCore.Mvc;
using TDS_Parking.DTOs;
using TDS_Parking.Interfaces;
using TDS_Parking.Validation;

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
        [VehicleRegFilter]
        [VehicleTypeFilterAttribute]
        public ActionResult<ParkingEntryDTO> enterParking([FromBody] VehicleDTO vehicle)
        {
            return Ok(parkingService.EnterParking(vehicle.VehicleReg, vehicle.VehicleType));
        }

        [HttpPost("exit")]
        [VehicleRegFilter]
        public ActionResult<ParkingChargeDTO> exitParking([FromBody] VehicleDTO vehicle)
        {
            return Ok(parkingService.ExitParking(vehicle.VehicleReg));
        }
    }
}
