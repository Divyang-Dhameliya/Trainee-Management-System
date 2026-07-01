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

    [HttpGet("ready")]
    public async Task<IActionResult> GetSystemDependenciesStatus(CancellationToken cancellationToken)
    {
        HealthReport report = await _healthCheckService.CheckHealthAsync(
            predicate: check => check.Tags.Contains("ready"), 
            cancellationToken: cancellationToken
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

    [HttpGet("live")]
    public async Task<IActionResult> GetSystemStatus(CancellationToken cancellationToken)
    {
        return Ok("Server running.");
    }
}
