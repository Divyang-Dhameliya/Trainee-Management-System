using System.Net;
using TraineeManagement.Api.Data;
using TraineeManagement.Api.DTO.ProcessingJobDTO;
using TraineeManagement.Api.Enum.ProcessingJob;
using TraineeManagement.Api.Models;

public class ProcessingJobService : IProcessingJobService
{
    private readonly AppDbContext _context;
    private readonly ILogger<ProcessingJobService> _logger;

    public ProcessingJobService(AppDbContext context, ILogger<ProcessingJobService> logger)
    { 
        _context = context; 
        _logger = logger;
    }

    public async Task CreateProcessingJob(ProcessingJobModel processingJob)
    {
        _context.ProcessingJobs.Add(processingJob);
        await _context.SaveChangesAsync();
    }

    public ProcessingJobModel GetProcessingJobByCorrelationId(Guid id)
    {
        ProcessingJobModel? processingjob = _context.ProcessingJobs.FirstOrDefault(
            processingjob => processingjob.CorrelationId == id
        );

        if(processingjob == null)
        {
            _logger.LogInformation("ProcessingJob not found with given CorrelationID: {Id}", id);
            throw new HttpStatusException(HttpStatusCode.NotFound, "ProcessingJob not found with given ID.");
        }

        return processingjob;
    }

    public async Task<ProcessingJobResponseModel> GetProcessingJobById(long id)
    {
        ProcessingJobModel? processingjob = await _context.ProcessingJobs.FindAsync(id);
        if(processingjob == null)
        {
            _logger.LogInformation("ProcessingJob not found with given ID: {Id}", id);
            throw new HttpStatusException(HttpStatusCode.NotFound, "ProcessingJob not found with given ID.");
        }

        return new ProcessingJobResponseModel
        {
            Status = processingjob.Status,
            Attempts = processingjob.Attempts,
            ErrorSummary = processingjob.ErrorSummary,
            CorrelationId = processingjob.CorrelationId,
            StartedAt = processingjob.StartedAt,
            CompletedAt = processingjob.CompletedAt
        };
    }

    public async Task UpdateProcessingJobById(int id, ProcessingJobEnum status)
    {
        ProcessingJobModel? processingjob = await _context.ProcessingJobs.FindAsync(id);

        if(processingjob == null)
        {
            _logger.LogInformation("ProcessingJob not found with given ID: {Id}", id);
            throw new HttpStatusException(HttpStatusCode.NotFound, "ProcessingJob not found with given ID.");
        }

        processingjob.Status = status;

        await _context.SaveChangesAsync();
    }
}