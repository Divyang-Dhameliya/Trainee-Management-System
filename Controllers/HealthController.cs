using Microsoft.AspNetCore.Mvc;

namespace TraineeManagement.Api.Controllers;

[ApiController]
[Route("api/health")]
public class HealthController : ControllerBase
{
    [HttpGet(Name = "GetHealth")]
    public IActionResult Get()
    {
        var res = new { status = "running", application = "Trainee Management API", timestamp = DateTime.UtcNow};

        return Ok(res);
    }
}
