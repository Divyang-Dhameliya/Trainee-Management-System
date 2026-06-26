using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TraineeManagement.Api.DTO.ReviewDTO;
using TraineeManagement.Api.Service.ReviewInterface;

namespace TraineeManagement.Api.Controllers;

[ApiController]
[Route("/api/processing-jobs")]
[Authorize]
public class ProcessingJobController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ProcessingJobController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] long id)
    {
        ReviewResponseModel? Review = await _reviewService.GetReviewById(id);

        return Ok(Review);
    }
}
