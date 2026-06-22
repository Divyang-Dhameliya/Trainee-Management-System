using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TraineeManagement.Api.Constants;
using TraineeManagement.Api.Enum.Submission;

namespace TraineeManagement.Api.Models;

public class SubmissionModel
{
    [Key]
    public long Id { get; set; }

    [Required(ErrorMessage = SubmissionConstants.TaskIdRequiredErrorMessage)]
    public long TaskAssignmentId { get; set; }

    [ForeignKey(nameof(TaskAssignmentId))]
    public TaskAssignmentModel? TaskAssignment { get; set; }

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

    public ICollection<SubmissionFile> Files { get; set; } = new List<SubmissionFile>();

    public SubmissionModel() { }
    public SubmissionModel(long TaskAssignmentId, string? SubmissionUrl, string? Notes, DateTime? SubmittedDate, SubmissionStatusEnum? Status)
    {
        this.TaskAssignmentId = TaskAssignmentId;
        this.SubmissionUrl = SubmissionUrl;
        this.Notes = Notes;
        this.SubmittedDate = SubmittedDate;
        this.Status = Status;
    }
}