using System.ComponentModel.DataAnnotations;
using TraineeManagement.Api.Enum.Trainee;
using TraineeManagement.Api.Constants;

namespace TraineeManagement.Api.Models;

public class TraineeModel
{
    public long Id { get; set; }

    [Required(ErrorMessage = TraineeConstants.FirstNameRequiredErrorMessage)]
    [StringLength(TraineeConstants.MaxLength, ErrorMessage = TraineeConstants.FirstNameMaxLengthErrorMessage)]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = TraineeConstants.LastNameRequiredErrorMessage)]
    [StringLength(TraineeConstants.MaxLength, ErrorMessage = TraineeConstants.LastNameMaxLengthErrorMessage)]
    public string? LastName { get; set; }

    [Required(ErrorMessage = TraineeConstants.EmailRequiredErrorMessage)]
    [EmailAddress(ErrorMessage = TraineeConstants.EmailValidateErrorMessage)]
    public string? Email { get; set; }

    [Required(ErrorMessage = TraineeConstants.TechStackRequiredErrorMessage)]
    public string? TechStack { get; set; }

    [Required(ErrorMessage = TraineeConstants.StatusRequiredErrorMessage)]
    [EnumDataType(typeof(TraineeStatus), ErrorMessage = TraineeConstants.StatusValidateErrorMessage)]
    public TraineeStatus? Status { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    public TraineeModel() { }
    public TraineeModel(long id, string? firstName, string? lastName, string? email, string? techstack, TraineeStatus? status)
    {
        Id = id;
        CreatedDate = DateTime.UtcNow;
        UpdatedDate = DateTime.UtcNow;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        TechStack = techstack;
        Status = status;
    }
}