using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TraineeManagement.Api.DTO.SubmissionDTO;
using TraineeManagement.Api.Service.SubmissionInterface;

namespace TraineeManagement.Api.Controllers;

[ApiController]
[Route("/api/submissions")]
[Authorize]
public class SubmissionController : ControllerBase
{
    private readonly ISubmissionService _submissionService;
    private readonly ILogger<SubmissionController> _logger;

    public SubmissionController(ISubmissionService submissionService, ILogger<SubmissionController> logger)
    {
        _submissionService = submissionService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        _logger.LogInformation("HTTP GET received for getSubmission.");
        
        List <SubmissionResponseModel> Submissions = await _submissionService.GetSubmissions();

        _logger.LogInformation("GetSubmissions completed successfully.");
        
        return Ok(Submissions);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] long id)
    {
        _logger.LogInformation("HTTP GET received for getSubmissionByID. SubmissionId: {Submissionid}", id); 
        SubmissionResponseModel? Submission = await _submissionService.GetSubmissionById(id);

        _logger.LogInformation("GetSubmissionByID completed successfully.");
        return Ok(Submission);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateSubmissionRequestModel Submission)
    {
        _logger.LogInformation("HTTP POST received for CreateSubmission.");
        SubmissionResponseModel newSubmission = await _submissionService.CreateSubmission(Submission);
        _logger.LogInformation("HTTP POST CreateSubmission completed successfully.");

        return Ok(newSubmission);
    }
}
