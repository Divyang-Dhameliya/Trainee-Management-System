using TraineeManagement.Api.Enum.Review;

namespace TraineeManagement.Api.DTO.ReviewDTO;

public class ReviewResponseModel
{
    public long Id { get; set; }

    public long SubmissionId { get; set; }

    public long MentorId { get; set; }

    public string? Feedback { get; set; }

    public int? Score { get; set; }

    public ReviewStatusEnum? ReviewStatus { get; set; }

    public DateTime? ReviewDate { get; set; }
    
    public ReviewResponseModel() { }
    public ReviewResponseModel(long Id, long SubmissionId, long MentorId, string? Feedback, int? Score, ReviewStatusEnum? ReviewStatus, DateTime? ReviewDate)
    {
        this.Id = Id;
        this.SubmissionId = SubmissionId;
        this.MentorId = MentorId;
        this.Feedback = Feedback;
        this.Score = Score;
        this.ReviewDate = ReviewDate;
        this.ReviewStatus = ReviewStatus;
    }
}