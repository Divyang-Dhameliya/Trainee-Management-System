public interface ISubmissionProcessingService
{
    Task ProcessAsync(SubmissionProcessingRequested message);

    Task UpdateProcessingJob(Guid correlationId, string erroMessage, int Attempt);
}