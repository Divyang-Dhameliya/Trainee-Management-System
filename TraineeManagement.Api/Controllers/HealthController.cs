using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TraineeManagement.Api.Constants;
namespace TraineeManagement.Api.Controllers;

[ApiController]
[Route("api/health")]
public class HealthController : ControllerBase
{
    private readonly HealthCheckService _healthCheckService;

    public HealthController(HealthCheckService healthCheckService)
    {
        _healthCheckService = healthCheckService;
    }

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

    [HttpGet("status")]
    [Authorize(Roles = "Admin")] 
    public async Task<IActionResult> GetSystemStatus(CancellationToken ct)
    {
        HealthReport report = await _healthCheckService.CheckHealthAsync(
            predicate: check => check.Tags.Contains("ready"), 
            cancellationToken: ct
        );

        var clientResponse = new
        {
            OverallStatus = report.Status.ToString(),
            TotalExecutionTimeMs = report.TotalDuration.TotalMilliseconds,
            Systems = report.Entries.Select(e => new
            {
                Component = e.Key,
                Status = e.Value.Status.ToString()
            })
        };

        if (report.Status == HealthStatus.Unhealthy)
        {
            return StatusCode(503, clientResponse);
        }

        return Ok(clientResponse);
    }
}
