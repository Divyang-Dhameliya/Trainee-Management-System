using Microsoft.AspNetCore.Mvc;
using TraineeManagement.Api.Constants;
namespace TraineeManagement.Api.Controllers;

[ApiController]
[Route("api/health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new 
        {
            status = ApplicationConstants.ApplicationStatus,
            application = ApplicationConstants.ApplicationName,
            timestamp = DateTime.UtcNow
        });
    }
}
