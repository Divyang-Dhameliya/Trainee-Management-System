namespace TraineeManagement.Api.Constants;

public static class ReviewConstants 
{
    public const int MaxLength = 50;
    public const string SubmissionIdRequiredErrorMessage = "SubmissionId is required";
    public const string MentorIdRequiredErrorMessage = "MentorId is required";
    public const string FeedbackRequiredErrorMessage = "Feedback is required";
    public const string FeedbackMaxLengthErrorMessage = "Feedback can't have more than 50 characters";
    public const string ReviewDateRequiredErrorMessage = "ReviewDate is required";
    public const string StatusRequiredErrorMessage = "Status is required";
    public const string StatusValidateErrorMessage = "Review status can be either Accepted / ChangesRequired / Rejected";
}