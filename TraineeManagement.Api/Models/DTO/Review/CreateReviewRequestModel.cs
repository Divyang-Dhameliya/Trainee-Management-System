using System.ComponentModel.DataAnnotations;
using TraineeManagement.Api.Constants;
using TraineeManagement.Api.Enum.Review;

namespace TraineeManagement.Api.DTO.ReviewDTO;

public class CreateReviewRequestModel
{
    [Required(ErrorMessage = ReviewConstants.SubmissionIdRequiredErrorMessage)]
    public long SubmissionId { get; set; }

    [Required(ErrorMessage = ReviewConstants.MentorIdRequiredErrorMessage)]
    public long MentorId { get; set; }

    [Required(ErrorMessage = ReviewConstants.FeedbackRequiredErrorMessage)]
    [StringLength(ReviewConstants.MaxLength, ErrorMessage = ReviewConstants.FeedbackMaxLengthErrorMessage)]
    public string? Feedback { get; set; }

    public int? Score { get; set; }

    [Required(ErrorMessage = ReviewConstants.StatusRequiredErrorMessage)]
    [EnumDataType(typeof(ReviewStatusEnum), ErrorMessage = ReviewConstants.StatusValidateErrorMessage)]
    public ReviewStatusEnum? ReviewStatus { get; set; }

    [Required(ErrorMessage = ReviewConstants.ReviewDateRequiredErrorMessage)]
    public DateTime? ReviewDate { get; set; }
    
    public CreateReviewRequestModel() { }
    public CreateReviewRequestModel(long SubmissionId, long MentorId, string? Feedback, int? Score, ReviewStatusEnum? ReviewStatus, DateTime? ReviewDate)
    {
        this.SubmissionId = SubmissionId;
        this.MentorId = MentorId;
        this.Feedback = Feedback;
        this.Score = Score;
        this.ReviewDate = ReviewDate;
        this.ReviewStatus = ReviewStatus;
    }
}