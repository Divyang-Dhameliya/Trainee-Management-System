using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TraineeManagement.Api.Constants;
using TraineeManagement.Api.Enum.Review;

namespace TraineeManagement.Api.Models;

public class ReviewModel
{
    [Key]
    public long Id { get; set; }

    [Required(ErrorMessage = ReviewConstants.SubmissionIdRequiredErrorMessage)]
    public long SubmissionId { get; set; }

    [ForeignKey(nameof(SubmissionId))]
    public SubmissionModel? Submission { get; set; }

    [Required(ErrorMessage = ReviewConstants.MentorIdRequiredErrorMessage)]
    public long MentorId { get; set; }

    [ForeignKey(nameof(MentorId))]
    public MentorModel? Mentor { get; set; }

    [Required(ErrorMessage = ReviewConstants.FeedbackRequiredErrorMessage)]
    [StringLength(ReviewConstants.MaxLength, ErrorMessage = ReviewConstants.FeedbackMaxLengthErrorMessage)]
    public string? Feedback { get; set; }

    public int? Score { get; set; }

    [Required(ErrorMessage = ReviewConstants.StatusRequiredErrorMessage)]
    [EnumDataType(typeof(ReviewStatusEnum), ErrorMessage = ReviewConstants.StatusValidateErrorMessage)]
    public ReviewStatusEnum? ReviewStatus { get; set; }

    [Required(ErrorMessage = ReviewConstants.ReviewDateRequiredErrorMessage)]
    public DateTime? ReviewDate { get; set; }
    
    public ReviewModel() { }
    public ReviewModel(long SubmissionId, long MentorId, string? Feedback, int? Score, ReviewStatusEnum? ReviewStatus, DateTime? ReviewDate)
    {
        this.SubmissionId = SubmissionId;
        this.MentorId = MentorId;
        this.Feedback = Feedback;
        this.Score = Score;
        this.ReviewDate = ReviewDate;
        this.ReviewStatus = ReviewStatus;
    }
}