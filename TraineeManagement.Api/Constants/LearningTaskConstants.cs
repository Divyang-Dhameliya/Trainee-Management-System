namespace TraineeManagement.Api.Constants;

public static class LearningTaskConstants
{
    public const int MaxLength = 50;
    public const string TitleRequiredErrorMessage = "Title is required";
    public const string TitleMaxLengthErrorMessage = "Title can't have more than 50 characters";
    public const string DescriptionRequiredErrorMessage = "Description is required";
    public const string DescriptionMaxLengthErrorMessage = "Description can't have more than 50 characters";
    public const string ExpectedTechStackRequiredErrorMessage = "Expected Tech-stack is required";
    public const string DueDateRequiredErrorMessage = "DueDate is required";
    public const string StatusRequiredErrorMessage = "Status is required";  
    public const string StatusValidateErrorMessage = "Task status can be either Draft, Published or Closed";
}