using System.ComponentModel.DataAnnotations;
using TraineeManagement.Api.Constants;
using TraineeManagement.Api.Enum.ProcessingJob;

namespace TraineeManagement.Api.Models;

public class ProcessingJobModel
{
    [Key]
    public long Id { get; set; }

    [Required(ErrorMessage = ProcessingJobConstants.StatusRequiredErrorMessage)]
    [EnumDataType(typeof(ProcessingJobEnum), ErrorMessage = ProcessingJobConstants.StatusValidateErrorMessage)]
    public ProcessingJobEnum? Status { get; set; }
    
    [Required(ErrorMessage = ProcessingJobConstants.AttemptsRequiredErrorMessage)]
    public int Attempts { get; set; } = 0;

    [StringLength(ProcessingJobConstants.MaxLength, ErrorMessage = ProcessingJobConstants.ErrorSummaryMaxLengthErrorMessage)]
    public string? ErrorSummary { get; set; }

    [Required(ErrorMessage = ProcessingJobConstants.CorrelationIdRequiredErrorMessage)]
    public Guid CorrelationId { get; set; }

    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public ProcessingJobModel() { }
}