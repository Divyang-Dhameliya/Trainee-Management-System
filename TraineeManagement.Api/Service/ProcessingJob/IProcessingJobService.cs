using TraineeManagement.Api.DTO.ProcessingJobDTO;
using TraineeManagement.Api.Enum.ProcessingJob;
using TraineeManagement.Api.Models;

public interface IProcessingJobService
{
    Task CreateProcessingJob(ProcessingJobModel processingJob);

    ProcessingJobModel GetProcessingJobByCorrelationId(Guid id);

    Task<ProcessingJobResponseModel> GetProcessingJobById(long id);

    Task UpdateProcessingJobById(int id, ProcessingJobEnum status);
}