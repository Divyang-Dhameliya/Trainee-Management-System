using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TraineeManagement.Api.Constants;
using TraineeManagement.Api.Enum.Submission;

namespace TraineeManagement.Api.DTO.SubmissionDTO;

public class CreateSubmissionRequestModel
{
    [Required(ErrorMessage = SubmissionConstants.TaskIdRequiredErrorMessage)]
    public long TaskAssignmentId { get; set; }

    [Required(ErrorMessage = SubmissionConstants.SubmissionUrlRequiredErrorMessage)]
    [StringLength(SubmissionConstants.MaxLength, ErrorMessage = SubmissionConstants.SubmissionUrlMaxLengthErrorMessage)]
    public string? SubmissionUrl { get; set; }

    [Required(ErrorMessage = SubmissionConstants.NotesRequiredErrorMessage)]
    [StringLength(SubmissionConstants.MaxLength, ErrorMessage = SubmissionConstants.NotesMaxLengthErrorMessage)]
    public string? Notes { get; set; }  

    [Required(ErrorMessage = SubmissionConstants.SubmittedDateRequiredErrorMessage)]
    public DateTime? SubmittedDate { get; set; }

    [Required(ErrorMessage = SubmissionConstants.StatusRequiredErrorMessage)]
    [EnumDataType(typeof(SubmissionStatusEnum), ErrorMessage = SubmissionConstants.StatusValidateErrorMessage)]
    public SubmissionStatusEnum? Status { get; set; }
    
    public CreateSubmissionRequestModel() { }
    public CreateSubmissionRequestModel(long TaskAssignmentId, string? SubmissionUrl, string? Notes, DateTime? SubmittedDate, SubmissionStatusEnum? Status)
    {
        this.TaskAssignmentId = TaskAssignmentId;
        this.SubmissionUrl  = SubmissionUrl;
        this.Notes = Notes;
        this.SubmittedDate = SubmittedDate;
        this.Status = Status;
    }
}