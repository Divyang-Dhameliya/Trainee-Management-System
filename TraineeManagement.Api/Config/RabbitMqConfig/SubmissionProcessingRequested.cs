public class SubmissionProcessingRequested
{
    public Guid MessageId { get; set; }
    public Guid CorrelationId { get; set; }
    public long SubmissionId { get; set; }
    public long FileId { get; set; }
    public DateTime RequestedAt { get; set; }
    public int ContractVersion { get; set; } = 1;
}