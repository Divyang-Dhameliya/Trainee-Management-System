using System.ComponentModel.DataAnnotations;
using TraineeManagement.Api.Enum.User;
using TraineeManagement.Api.Constants;
using Microsoft.EntityFrameworkCore;

namespace TraineeManagement.Api.Models;

[Index(nameof(UserName), IsUnique = true)]
public class UserModel
{
    [Key]
    public long Id { get; set; }

    [Required(ErrorMessage = UserConstants.UserNameRequiredErrorMessage)]
    [StringLength(UserConstants.MaxLength, ErrorMessage = UserConstants.UserNameMaxLengthErrorMessage)]
    public string? UserName { get; set; }

    [Required(ErrorMessage = UserConstants.EmailRequiredErrorMessage)]
    [EmailAddress(ErrorMessage = UserConstants.EmailValidateErrorMessage)]
    public string? Email { get; set; }

    [Required(ErrorMessage = UserConstants.PasswordHashRequiredErrorMessage)]
    public string? PasswordHash { get; set; }

    [Required(ErrorMessage = UserConstants.RoleRequiredErrorMessage)]
    [EnumDataType(typeof(UserRole), ErrorMessage = UserConstants.RoleValidateErrorMessage)]
    public UserRole? Role { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    public UserModel() { }
    public UserModel(string? userName, string? email, string? passwordHash, UserRole? role)
    {
        CreatedDate = DateTime.UtcNow;
        UpdatedDate = DateTime.UtcNow;
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
    }
}