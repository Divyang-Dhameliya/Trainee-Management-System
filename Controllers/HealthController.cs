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
            status = TraineeConstants.ApplicationStatus,
            application = TraineeConstants.ApplicationName,
            timestamp = DateTime.UtcNow
        });
    }
}
