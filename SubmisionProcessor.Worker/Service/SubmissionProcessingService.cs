using System.Net;
using Microsoft.EntityFrameworkCore;
using TraineeManagement.Api.Data;
using TraineeManagement.Api.Enum.ProcessingJob;
using TraineeManagement.Api.Enum.Submission;
using TraineeManagement.Api.Models;

public class SubmissionProcessingService : ISubmissionProcessingService
{
    private readonly AppDbContext _context;
    private readonly IFileStorageService _fileStorageService;
    private readonly IProcessingJobService _processingJobService;

    public SubmissionProcessingService(AppDbContext context, IFileStorageService fileStorageService, IProcessingJobService processingJobService)
    {
        _context = context;
        _fileStorageService = fileStorageService;
        _processingJobService = processingJobService;
    }

    public async Task ProcessAsync(SubmissionProcessingRequested message)
    {
        SubmissionFile? file = await _context.SubmissionFiles
                .Include(x => x.Submission)
                .FirstOrDefaultAsync(
                    x => x.Id == message.FileId
        );

        ProcessingJobModel processingJobModel = _processingJobService.GetProcessingJobByCorrelationId(message.CorrelationId);
        
        try
        {
            if (file == null)
                throw new PermenentException($"File not found with given Id: {message.FileId}");

            if(file.Submission != null)
            {
                file.Submission.Status = SubmissionStatusEnum.Processing;
            }

            processingJobModel.Status = ProcessingJobEnum.Processing;

            await _context.SaveChangesAsync();

            using Stream stream = _fileStorageService.OpenReadAsync(file.StorageFileName);

            // Simulate long processing
            await Task.Delay(TimeSpan.FromSeconds(10));

            if(file.Submission != null)
            {
                file.Submission.Status = SubmissionStatusEnum.Completed;
            }

            processingJobModel.Status = ProcessingJobEnum.Completed;
            processingJobModel.CompletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
        catch
        {
            if(file == null)
            {
                SubmissionModel? submission = await _context.Submissions
                .FirstOrDefaultAsync(
                    x => x.Id == message.SubmissionId
                );

                submission.Status = SubmissionStatusEnum.Failed;
            }
            else if(file.Submission != null)
            {
                file.Submission.Status = SubmissionStatusEnum.Failed;
            }

            processingJobModel.Status = ProcessingJobEnum.Failed;

            await _context.SaveChangesAsync();
            throw;
        }
    }

    public async Task UpdateProcessingJob(Guid correlationId, string erroMessage, int Attempt)
    {
        ProcessingJobModel processingJobModel = _processingJobService.GetProcessingJobByCorrelationId(correlationId);

        processingJobModel.ErrorSummary = erroMessage;
        processingJobModel.Attempts = Attempt;

        await _context.SaveChangesAsync();
    }
}