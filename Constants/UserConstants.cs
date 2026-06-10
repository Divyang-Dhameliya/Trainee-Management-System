namespace TraineeManagement.Api.Constants;

public static class UserConstants
{
    public const int MaxLength = 50;
    public const string UserNameRequiredErrorMessage = "Username is required";
    public const string UserNameMaxLengthErrorMessage = "Username can't have more than 50 characters";
    public const string EmailRequiredErrorMessage = "Email is required";
    public const string EmailValidateErrorMessage = "Valid email is required";
    public const string PasswordHashRequiredErrorMessage = "Password is required";
    public const string RoleRequiredErrorMessage = "Role is required";
    public const string RoleValidateErrorMessage = "User role can be either Admin, Mentor or Trainee";
}