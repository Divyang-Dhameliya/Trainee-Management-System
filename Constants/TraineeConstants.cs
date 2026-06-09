namespace TraineeManagement.Api.Constants;

public static class TraineeConstants
{
    public const string ApplicationStatus = "running";
    public const string ApplicationName = "Trainee Management API";
    public const int COUNT_INCREMENTOR_ONE = 1;
    public const int MaxLength = 50;
    public const string FirstNameRequiredErrorMessage = "First name is required";
    public const string FirstNameMaxLengthErrorMessage = "Firstname can't have more than 50 characters";
    public const string LastNameRequiredErrorMessage = "Last name is required";
    public const string LastNameMaxLengthErrorMessage = "Lastname can't have more than 50 characters";
    public const string EmailRequiredErrorMessage = "Email is required";
    public const string EmailValidateErrorMessage = "Valid email is required";
    public const string TechStackRequiredErrorMessage = "Tech-stack is required";
    public const string StatusRequiredErrorMessage = "Status is required";
    public const string StatusValidateErrorMessage = "Trainee status can be either Active or Inactive";
}