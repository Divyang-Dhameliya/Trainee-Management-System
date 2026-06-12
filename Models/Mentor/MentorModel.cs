using System.ComponentModel.DataAnnotations;
using TraineeManagement.Api.Constants;
using TraineeManagement.Api.Enum.Mentor;

namespace TraineeManagement.Api.Models;

public class MentorModel
{
    [Key]
    public long Id { get; set; }

    [Required(ErrorMessage = MentorConstants.FirstNameRequiredErrorMessage)]
    [StringLength(MentorConstants.MaxLength, ErrorMessage = MentorConstants.FirstNameMaxLengthErrorMessage)]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = MentorConstants.LastNameRequiredErrorMessage)]
    [StringLength(MentorConstants.MaxLength, ErrorMessage = MentorConstants.LastNameMaxLengthErrorMessage)]
    public string? LastName { get; set; }

    [Required(ErrorMessage = MentorConstants.EmailRequiredErrorMessage)]
    [EmailAddress(ErrorMessage = MentorConstants.EmailValidateErrorMessage)]
    public string? Email { get; set; }

    [Required(ErrorMessage = MentorConstants.ExpertiseRequiredErrorMessage)]
    public string? Expertise { get; set; }

    [Required(ErrorMessage = MentorConstants.StatusRequiredErrorMessage)]
    [EnumDataType(typeof(MentorStatus), ErrorMessage = MentorConstants.StatusValidateErrorMessage)]
    public MentorStatus? Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    public MentorModel() { }
    public MentorModel(string? FirstName, string? LastName, string? Email, string? Expertise, MentorStatus? status)
    {
        CreatedDate = DateTime.UtcNow;
        UpdatedDate = DateTime.UtcNow;
        this.FirstName = FirstName;
        this.LastName = LastName;
        this.Email = Email;
        this.Expertise = Expertise;
        Status = status;
    }
}