namespace TraineeManagement.Api.Constants;

public static class SubmissionConstants 
{
    public const int MaxLength = 50;

    public const string TaskIdRequiredErrorMessage = "TaskId is required";
    public const string SubmissionUrlRequiredErrorMessage = "SubmissionUrl is required";
    public const string SubmissionUrlMaxLengthErrorMessage = "SubmissionUrl can't have more than 50 characters";
    public const string NotesRequiredErrorMessage = "Notes is required";
    public const string NotesMaxLengthErrorMessage = "Notes can't have more than 50 characters";
    public const string SubmittedDateRequiredErrorMessage = "SubmittedDate is required";
    public const string StatusRequiredErrorMessage = "Status is required";
    public const string StatusValidateErrorMessage = "Submission status can be either Submitted / Resubmitted / Queued / Processing / Completed or Failed";
}