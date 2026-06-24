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

    public ReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        
        List <ReviewResponseModel> Reviews = await _reviewService.GetReviews();
        
        return Ok(Reviews);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] long id)
    {
        ReviewResponseModel? Review = await _reviewService.GetReviewById(id);

        return Ok(Review);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateReviewRequestModel Review)
    {
        ReviewResponseModel newReview = await _reviewService.CreateReview(Review);

        return Ok(newReview);
    }
}
