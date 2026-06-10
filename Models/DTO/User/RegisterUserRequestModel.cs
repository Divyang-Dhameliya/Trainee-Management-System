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

    [Required(ErrorMessage = UserConstants.RoleRequiredErrorMessage)]
    [EnumDataType(typeof(UserRole), ErrorMessage = UserConstants.RoleValidateErrorMessage)]
    public UserRole? Role { get; set; }

    public RegisterUserRequestModel(string? userName, string? email, string? password, UserRole? role)
    {
        UserName = userName;
        Email = email;
        Password = password;
        Role = role;
    }
}