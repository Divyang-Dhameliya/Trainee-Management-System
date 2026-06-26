
using TraineeManagement.Api.DTO.ReviewDTO;

namespace TraineeManagement.Api.Service.ReviewInterface;

public interface IReviewService
{
    Task<List<ReviewResponseModel>> GetReviews();

    Task<ReviewResponseModel?> GetReviewById(long id);

    Task<ReviewResponseModel> CreateReview(CreateReviewRequestModel Review);
}