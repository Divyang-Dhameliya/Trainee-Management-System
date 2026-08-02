using System.ComponentModel.DataAnnotations;
using TraineeManagement.Api.Enum.User;
using TraineeManagement.Api.Constants;
using Microsoft.EntityFrameworkCore;

namespace TraineeManagement.Api.DTO.UserDTO;

[Index(nameof(UserName), IsUnique = true)]
public class RegisterUserRequestModel
{
    [Required(ErrorMessage = UserConstants.UserNameRequiredErrorMessage)]
    [StringLength(UserConstants.MaxLength, ErrorMessage = UserConstants.UserNameMaxLengthErrorMessage)]
    public string? UserName { get; set; }

    [Required(ErrorMessage = UserConstants.EmailRequiredErrorMessage)]
    [EmailAddress(ErrorMessage = UserConstants.EmailValidateErrorMessage)]
    public string? Email { get; set; }

    [Required(ErrorMessage = UserConstants.PasswordHashRequiredErrorMessage)]
    public string? Password { get; set; }

    // Only Trainee or Mentor may be requested here. Admin can never be
    // self-assigned — AuthService.RegisterUser enforces this regardless
    // of what value is sent.
    [Required(ErrorMessage = UserConstants.RoleRequiredErrorMessage)]
    public UserRole? Role { get; set; }

    // Shared profile fields
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    // Required only when Role == Trainee
    public string? TechStack { get; set; }

    // Required only when Role == Mentor
    public string? Expertise { get; set; }

    public RegisterUserRequestModel(string? userName, string? email, string? password, UserRole? role, string? firstName, string? lastName, string? techStack, string? expertise)
    {
        UserName = userName;
        Email = email;
        Password = password;
        Role = role;
        FirstName = firstName;
        LastName = lastName;
        TechStack = techStack;
        Expertise = expertise;
    }
}