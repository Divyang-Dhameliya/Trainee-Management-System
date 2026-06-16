using System.Net;
using Microsoft.EntityFrameworkCore;
using TraineeManagement.Api.Data;
using TraineeManagement.Api.DTO.ReviewDTO;
using TraineeManagement.Api.Models;
using TraineeManagement.Api.Service.ReviewInterface;

public class ReviewService : IReviewService
{

    private readonly AppDbContext _context;

    public ReviewService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ReviewResponseModel> CreateReview(CreateReviewRequestModel review)
    {
        ReviewModel newReview = new ReviewModel(
            review.SubmissionId,
            review.MentorId,
            review.Feedback,
            review.Score,
            review.ReviewStatus,
            review.ReviewDate
        );

        _context.Reviews.Add(newReview);
        await _context.SaveChangesAsync();

        ReviewResponseModel ReviewResponseModel = new ReviewResponseModel(
            newReview.Id,
            newReview.SubmissionId,
            newReview.MentorId,
            newReview.Feedback,
            newReview.Score,
            newReview.ReviewStatus,
            newReview.ReviewDate
        );

        return ReviewResponseModel;
    }

    public async Task<ReviewResponseModel?> GetReviewById(long id)
    {
        ReviewModel? review = await _context.Reviews.FindAsync(id);

        if(review == null)
        {
            throw new HttpStatusException(HttpStatusCode.NotFound, "Review not found with given ID.");
        }

        return new ReviewResponseModel(
            review.Id,
            review.SubmissionId,
            review.MentorId,
            review.Feedback,
            review.Score,
            review.ReviewStatus,
            review.ReviewDate
        );
    }

    public async Task<List<ReviewResponseModel>> GetReviews()
    {
        List<ReviewResponseModel> ReviewResponseModels = new([]);

        List<ReviewModel> reviews = await _context.Reviews.ToListAsync();

        foreach (ReviewModel review in reviews)
        {
            ReviewResponseModels.Add(
                new ReviewResponseModel(
                    review.Id,
                    review.SubmissionId,
                    review.MentorId,
                    review.Feedback,
                    review.Score,
                    review.ReviewStatus,
                    review.ReviewDate
                )
            );
        }

        return ReviewResponseModels;
    }
}