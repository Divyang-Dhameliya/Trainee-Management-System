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

    public SubmissionController(ISubmissionService submissionService)
    {
        _submissionService = submissionService;
    }

    [HttpGet("/api/submissions")]
    public async Task<IActionResult> Get()
    {        
        List <SubmissionResponseModel> Submissions = await _submissionService.GetSubmissions();
        
        return Ok(Submissions);
    }

    [HttpGet("/api/submissions/{id}")]
    public async Task<IActionResult> Get([FromRoute] long id)
    {
        SubmissionResponseModel? Submission = await _submissionService.GetSubmissionById(id);

        return Ok(Submission);
    }

    [HttpPost("/api/submissions")]
    public async Task<IActionResult> Post([FromBody] CreateSubmissionRequestModel Submission)
    {
        SubmissionResponseModel newSubmission = await _submissionService.CreateSubmission(Submission);

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

        return Accepted(result);
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
