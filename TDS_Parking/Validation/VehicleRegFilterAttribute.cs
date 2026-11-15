using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TDS_Parking.DTOs;

namespace TDS_Parking.Validation
{
    public class VehicleRegFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var vehicle = context.ActionArguments["vehicle"] as VehicleDTO;
            if (vehicle == null || string.IsNullOrEmpty(vehicle.VehicleReg))
            {
                var problemDetails = new ValidationProblemDetails(
                    new Dictionary<string, string[]>
                    {
                        { "VehicleReg", new[] { "Vehicle registration number is required." } }
                    }
                );
                context.Result = new BadRequestObjectResult(problemDetails);
                return;
            }
            else if (vehicle.VehicleReg.Length < 4 || vehicle.VehicleReg.Length > 12)
            {
                var problemDetails = new ValidationProblemDetails(
                    new Dictionary<string, string[]>
                    {
                        { "VehicleReg", new[] { "Vehicle registration number must be between 4 and 12 characters long." } }
                    }
                );
                context.Result = new BadRequestObjectResult(problemDetails);
                return;
            }
        }
    }
}
