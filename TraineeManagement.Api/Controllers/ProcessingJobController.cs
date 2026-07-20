using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TraineeManagement.Api.DTO.ProcessingJobDTO;

namespace TraineeManagement.Api.Controllers;

[ApiController]
[Route("/api/processing-jobs")]
[Authorize( Roles = "Admin")]
public class ProcessingJobController : ControllerBase
{
    private readonly IProcessingJobService _processingJobService;

    public ProcessingJobController(IProcessingJobService processingJobService)
    {
        _processingJobService = processingJobService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] long id)
    {
        ProcessingJobResponseModel? Review = await _processingJobService.GetProcessingJobById(id);

        return Ok(Review);
    }
}
