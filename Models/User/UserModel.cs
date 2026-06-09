using System.ComponentModel.DataAnnotations;
using TraineeManagement.Api.Enum.User;
using TraineeManagement.Api.Constants;

namespace TraineeManagement.Api.Models;

public class UserModel
{
    [Key]
    public long Id { get; set; }

    [Required(ErrorMessage = TraineeConstants.FirstNameRequiredErrorMessage)]
    [StringLength(TraineeConstants.MaxLength, ErrorMessage = TraineeConstants.FirstNameMaxLengthErrorMessage)]
    public string? UserName { get; set; }

    [Required(ErrorMessage = TraineeConstants.EmailRequiredErrorMessage)]
    [EmailAddress(ErrorMessage = TraineeConstants.EmailValidateErrorMessage)]
    public string? Email { get; set; }

    [StringLength(TraineeConstants.MaxLength, ErrorMessage = TraineeConstants.FirstNameMaxLengthErrorMessage)]
    public string? PasswordHash { get; set; }

    [Required(ErrorMessage = TraineeConstants.StatusRequiredErrorMessage)]
    [EnumDataType(typeof(UserRole), ErrorMessage = TraineeConstants.StatusValidateErrorMessage)]
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