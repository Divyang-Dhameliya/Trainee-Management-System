namespace TraineeManagement.Api.Constants;

public static class ProcessingJobConstants
{
    public const int MaxLength = 50;
    public const string StatusRequiredErrorMessage = "Status is required";
    public const string StatusValidateErrorMessage = "Processing status can be either Queued, Processing, Completed or Failed";
    public const string AttemptsRequiredErrorMessage = "Attempts is required";
    public const string ErrorSummaryMaxLengthErrorMessage = "ErrorSummary can't have more than 50 characters";
    public const string CorrelationIdRequiredErrorMessage = "CorrelationId is required";
    
}