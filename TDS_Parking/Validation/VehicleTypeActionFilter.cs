using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TDS_Parking.DTOs;
using TDS_Parking.Enums;

namespace TDS_Parking.Validation
{
    public class VehicleTypeActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var vehicle = context.ActionArguments["vehicle"] as VehicleDTO;

            if (vehicle == null || !Enum.IsDefined(typeof(VehicleType), vehicle.VehicleType))
            {
                var problemDetails = new ValidationProblemDetails(
                    new Dictionary<string, string[]>
                    {
                        { "VehicleType", new[] { "Invalid vehicle type." } }
                    }
                );
                context.Result = new BadRequestObjectResult(problemDetails);
                return;
            }
        }
    }
}
