using TraineeManagement.Api.Enum.ProcessingJob;

namespace TraineeManagement.Api.DTO.ProcessingJobDTO;

public class ProcessingJobResponseModel
{
    public ProcessingJobEnum? Status { get; set; }

    public int Attempts { get; set; }

    public string? ErrorSummary { get; set; }

    public Guid CorrelationId { get; set; }

    public DateTime StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }
}