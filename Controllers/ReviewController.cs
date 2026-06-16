using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TraineeManagement.Api.DTO.ReviewDTO;
using TraineeManagement.Api.Service.ReviewInterface;

namespace TraineeManagement.Api.Controllers;

[ApiController]
[Route("/api/reviews")]
[Authorize]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;
    private readonly ILogger<ReviewController> _logger;

    public ReviewController(IReviewService reviewService, ILogger<ReviewController> logger)
    {
        _reviewService = reviewService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        _logger.LogInformation("HTTP GET received for getReview.");
        
        List <ReviewResponseModel> Reviews = await _reviewService.GetReviews();

        _logger.LogInformation("GetReviews completed successfully.");
        
        return Ok(Reviews);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] long id)
    {
        _logger.LogInformation("HTTP GET received for getReviewByID. ReviewId: {Reviewid}", id); 
        ReviewResponseModel? Review = await _reviewService.GetReviewById(id);

        _logger.LogInformation("GetReviewByID completed successfully.");
        return Ok(Review);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateReviewRequestModel Review)
    {
        _logger.LogInformation("HTTP POST received for CreateReview.");
        ReviewResponseModel newReview = await _reviewService.CreateReview(Review);
        _logger.LogInformation("HTTP POST CreateReview completed successfully.");

        return Ok(newReview);
    }
}
