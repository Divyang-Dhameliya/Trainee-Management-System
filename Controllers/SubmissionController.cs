using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TraineeManagement.Api.DTO.SubmissionDTO;
using TraineeManagement.Api.Service.SubmissionInterface;
using TraineeManagement.Api.DTO.SubmissionFileDTO;

namespace TraineeManagement.Api.Controllers;

[ApiController]
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

    [HttpGet("/api/submissions")]
    public async Task<IActionResult> Get()
    {
        _logger.LogInformation("HTTP GET received for getSubmission.");
        
        List <SubmissionResponseModel> Submissions = await _submissionService.GetSubmissions();

        _logger.LogInformation("GetSubmissions completed successfully.");
        
        return Ok(Submissions);
    }

    [HttpGet("/api/submissions/{id}")]
    public async Task<IActionResult> Get([FromRoute] long id)
    {
        _logger.LogInformation("HTTP GET received for getSubmissionByID. SubmissionId: {Submissionid}", id); 
        SubmissionResponseModel? Submission = await _submissionService.GetSubmissionById(id);

        _logger.LogInformation("GetSubmissionByID completed successfully.");
        return Ok(Submission);
    }

    [HttpPost("/api/submissions")]
    public async Task<IActionResult> Post([FromBody] CreateSubmissionRequestModel Submission)
    {
        _logger.LogInformation("HTTP POST received for CreateSubmission.");
        SubmissionResponseModel newSubmission = await _submissionService.CreateSubmission(Submission);
        _logger.LogInformation("HTTP POST CreateSubmission completed successfully.");

        return Ok(newSubmission);
    }

    [HttpPost("/api/submissions/{submissionId}/files")]
    public async Task<IActionResult> Upload(long submissionId, [FromForm] IFormFile File, CancellationToken cancellationToken)
    {
        SubmissionFileResponseModel result = await _submissionService.UploadAsync(
            submissionId,
            File,
            cancellationToken
        );

        return CreatedAtAction(
            nameof(Get),
            new { id = result.Id },
            result
        );
    }

    [HttpGet("/api/submission-files/{fileId}/download")]
    public async Task<IActionResult> Download(long fileId, CancellationToken cancellationToken)
    {
        DownloadSubmissionFileResponseModel result = await _submissionService.DownloadAsync(
            fileId,
            cancellationToken
        );

        return File(
            result.Stream,
            result.ContentType,
            result.FileName
        );
    }

    [HttpDelete("/api/submission-files/{fileId}")]
    public async Task<IActionResult> Delete(long fileId, CancellationToken cancellationToken)
    {
        await _submissionService.DeleteAsync(
            fileId,
            cancellationToken
        );

        return NoContent();
    }
}
