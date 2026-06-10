using System.ComponentModel.DataAnnotations;
using TraineeManagement.Api.Constants;
using Microsoft.EntityFrameworkCore;

namespace TraineeManagement.Api.DTO.UserDTO;

[Index(nameof(UserName), IsUnique = true)]
public class LoginUserRequestModel
{
    [Required(ErrorMessage = UserConstants.UserNameRequiredErrorMessage)]
    [StringLength(UserConstants.MaxLength, ErrorMessage = UserConstants.UserNameMaxLengthErrorMessage)]
    public string? UserName { get; set; }

    [Required(ErrorMessage = UserConstants.PasswordHashRequiredErrorMessage)]
    public string? Password { get; set; }

    public LoginUserRequestModel(string? userName, string? password)
    {
        UserName = userName;
        Password = password;
    }
}