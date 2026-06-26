using System.ComponentModel;
using System.Net;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TraineeManagement.Api.Data;
using TraineeManagement.Api.DTO.SubmissionDTO;
using TraineeManagement.Api.DTO.SubmissionFileDTO;
using TraineeManagement.Api.Enum.ProcessingJob;
using TraineeManagement.Api.Enum.Submission;
using TraineeManagement.Api.Helpers;
using TraineeManagement.Api.Models;
using TraineeManagement.Api.Service.SubmissionInterface;

public class SubmissionService : ISubmissionService
{
    private readonly AppDbContext _context;
    private readonly FileStorageOptions _options;
    private readonly IFileStorageService _fileStorageService;
    private readonly ILogger<SubmissionService> _logger;
    private readonly ICacheService _cacheService;
    private readonly IMessagePublisher _messagePublisher;
    private readonly IProcessingJobService _proceessingJobService;
    public SubmissionService(
        AppDbContext context,
        IOptions<FileStorageOptions> options, 
        IFileStorageService fileStorageService, 
        ILogger<SubmissionService> logger, 
        ICacheService cacheService,
        IMessagePublisher messagePublisher,
        IProcessingJobService processingJobService
    )
    {
        _context = context;
        _options = options.Value;
        _fileStorageService = fileStorageService;
        _logger = logger;
        _cacheService = cacheService;
        _messagePublisher = messagePublisher;
        _proceessingJobService = processingJobService;
    }

    public async Task<SubmissionResponseModel> CreateSubmission(CreateSubmissionRequestModel submission)
    {
        SubmissionModel newSubmission = new SubmissionModel(
            submission.TaskAssignmentId,
            submission.SubmissionUrl,
            submission.Notes,
            submission.SubmittedDate,
            submission.Status
        );

        _context.Submissions.Add(newSubmission);
        await _context.SaveChangesAsync();

        await _cacheService.RemoveAsync(CacheKeys.SubmissionsAll);

        SubmissionResponseModel SubmissionResponseModel = new SubmissionResponseModel(
            newSubmission.Id,
            newSubmission.TaskAssignmentId,
            newSubmission.SubmissionUrl,
            newSubmission.Notes,
            newSubmission.SubmittedDate,
            newSubmission.Status
        );

        return SubmissionResponseModel;
    }

    public async Task<SubmissionResponseModel?> GetSubmissionById(long id)
    {
        string cacheKey = CacheKeys.SubmissionSummary(id);

        SubmissionResponseModel? cachedSubmission = await _cacheService.GetAsync<SubmissionResponseModel>(cacheKey);

        if (cachedSubmission != null)
        {
            _logger.LogInformation("Cache hit for {CacheKey}", cacheKey);
            return cachedSubmission;
        }

        _logger.LogInformation("Cache miss for {CacheKey}", cacheKey);

        SubmissionModel? submission = await _context.Submissions.FindAsync(id);

        if (submission == null)
        {
            _logger.LogInformation("Submission not found with given ID: {Id}", id);
            throw new HttpStatusException(HttpStatusCode.NotFound, "Submission not found with given ID.");
        }

        SubmissionResponseModel response = new SubmissionResponseModel(
            submission.Id,
            submission.TaskAssignmentId,
            submission.SubmissionUrl,
            submission.Notes,
            submission.SubmittedDate,
            submission.Status
        );

        await _cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(10));

        return response;
    }

    public async Task<List<SubmissionResponseModel>> GetSubmissions()
    {
        string cacheKey = CacheKeys.SubmissionsAll;

        List<SubmissionResponseModel>? cachedSubmissions = await _cacheService.GetAsync<List<SubmissionResponseModel>>(cacheKey);

        if (cachedSubmissions != null)
        {
            _logger.LogInformation("Cache hit for {CacheKey}", cacheKey);
            return cachedSubmissions;
        }

        _logger.LogInformation("Cache miss for {CacheKey}", cacheKey);

        List<SubmissionResponseModel> SubmissionResponseModels = new([]);

        List<SubmissionModel> submissions = await _context.Submissions.ToListAsync();

        foreach (SubmissionModel submission in submissions)
        {
            SubmissionResponseModels.Add(
                new SubmissionResponseModel(
                    submission.Id,
                    submission.TaskAssignmentId,
                    submission.SubmissionUrl,
                    submission.Notes,
                    submission.SubmittedDate,
                    submission.Status
                )
            );
        }

        await _cacheService.SetAsync(cacheKey, SubmissionResponseModels, TimeSpan.FromMinutes(10));

        return SubmissionResponseModels;
    }

    public async Task<SubmissionFileResponseModel> UploadAsync(long submissionId, IFormFile file, CancellationToken cancellationToken)
    {
        Guid correlationId = Guid.NewGuid();
    
        _logger.LogInformation("Upload request received. CorrelationId: {CorrelationId}, SubmissionId: {SubmissionId}, FileName: {FileName}",
            correlationId,
            submissionId,
            file.FileName
        );

        SubmissionModel? submission = await _context.Submissions.FirstOrDefaultAsync(
            submission => submission.Id == submissionId,
            cancellationToken
        );

        if (submission == null)
        {
            _logger.LogWarning("Submission Not Found with given Id. CorrelationId : {CorrelationId}, SubmissionId: {SubmissionId}, FileName: {FileName}",
                correlationId,
                submissionId,
                file.FileName
            );
            throw new HttpStatusException(HttpStatusCode.NotFound, "Submission not found.");
        }

        string extension = Path.GetExtension(file.FileName);

        if(file.Length == 0)
        {
            _logger.LogWarning("Uploaded File is Empty. CorrelationId: {CorrelationId}, SubmissionId: {SubmissionId}, FileName: {FileName}",
                correlationId,
                submissionId,
                file.FileName
            );
            throw new HttpStatusException(HttpStatusCode.BadRequest, "Uploaded File is Empty.");    
        }

        if (!_options.AllowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
        {
            _logger.LogWarning("Invalid File Extension. CorrelationId: {CorrelationId}, Extension : {Extension}, SubmissionId: {SubmissionId}, FileName: {FileName}",
                correlationId,
                extension,
                submissionId,
                file.FileName
            );
            throw new HttpStatusException(HttpStatusCode.BadRequest, "Invalid file extension.");
        }

        long maxSizeBytes = (long)_options.MaxFileSizeMb * 1024 * 1024;

        if (file.Length > maxSizeBytes)
        {
            _logger.LogWarning("File-Size Limit exceeds. CorrelationId: {CorrelationId}, UploadedFileSize: {UploadedFileSize}, SubmissionId: {SubmissionId}, FileName: {FileName}",
                correlationId,
                file.Length,
                submissionId,
                file.FileName
            );
            throw new HttpStatusException(HttpStatusCode.RequestEntityTooLarge, "File Size exceeds limit.");
        }

        string storageFileName = $"{Guid.NewGuid()}{extension}";

        try
        {
            string checksum;

            await using (Stream stream = file.OpenReadStream())
            {
                checksum = await ChecksumHelper.ComputeAsync(stream, cancellationToken);
            }

            bool duplicateExists = await _context.SubmissionFiles.AnyAsync(
                file => file.SubmissionId == submissionId &&
                file.Checksum == checksum
            );

            if (duplicateExists)
            {
                _logger.LogWarning("Duplicate File detected for same submission. CorrelationId: {CorrelationId}, Checksum: {Checksum}, SubmissionId: {SubmissionId}, FileName: {FileName}",
                    correlationId,
                    checksum,
                    submissionId,
                    file.FileName
                );
                throw new HttpStatusException(HttpStatusCode.Conflict, "An identical file already exists for this submission.");
            }

            await using (Stream stream = file.OpenReadStream())
            {
                await _fileStorageService.SaveAsync(
                    stream,
                    storageFileName,
                    cancellationToken
                );
            }

            _logger.LogWarning("File stored successfully. CorrelationId: {CorrelationId}, SubmissionId: {SubmissionId}, StorageFileName: {StorageFileName}",
                correlationId,
                submissionId,
                storageFileName
            );
            
            SubmissionFile submissionFile = new SubmissionFile
            {
                OriginalFileName = file.FileName,
                StorageFileName = storageFileName,
                ContentType = file.ContentType,
                SizeInBytes = file.Length,
                Checksum = checksum,
                UploadedAt = DateTime.UtcNow,
                SubmissionId = submissionId
            };

            _context.SubmissionFiles.Add(submissionFile);

            if(submissionFile.Submission !=null)
            {
                submissionFile.Submission.Status = SubmissionStatusEnum.Queued;
            }

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogWarning("SubmissionFile metadata persisted. CorrelationId: {CorrelationId}, SubmissionId: {SubmissionId}, FileName: {FileName}",
                correlationId,
                submissionId,
                file.FileName
            );

            await _proceessingJobService.CreateProcessingJob(
                new ProcessingJobModel
                {
                    Status = ProcessingJobEnum.Queued,
                    Attempts = 0,
                    CorrelationId = correlationId,
                    StartedAt = DateTime.UtcNow
                }
            );

            SubmissionProcessingRequested message = new SubmissionProcessingRequested
            {
                MessageId = Guid.NewGuid(),
                CorrelationId = correlationId,
                SubmissionId = submissionId,
                FileId = submissionFile.Id,
                // FileId = 1000,
                RequestedAt = DateTime.UtcNow
            };

            await _messagePublisher.PublishAsync(message);


            _logger.LogInformation(
                "Submission processing message published & ProcessingJob persisted. CorrelationId: {CorrelationId}, MessageId: {MessageId}, SubmissionId: {SubmisssionId}, FileId: {FileId}",
                message.CorrelationId,
                message.MessageId,
                message.SubmissionId,
                message.FileId
            );
            
            return new SubmissionFileResponseModel
            {
                Id = submissionFile.Id,
                OriginalFileName = submissionFile.OriginalFileName,
                ContentType = submissionFile.ContentType,
                SizeInBytes = submissionFile.SizeInBytes,
                UploadedAt = submissionFile.UploadedAt
            };
        }
        catch
        {
            await _fileStorageService.DeleteAsync(
                storageFileName,
                cancellationToken
            );

            throw;
        }
    }

    public async Task<DownloadSubmissionFileResponseModel> DownloadAsync(long fileId, CancellationToken cancellationToken = default)
    {
        SubmissionFile? submissionFile = await _context.SubmissionFiles.FirstOrDefaultAsync(
            submissionFile => submissionFile.Id == fileId,
            cancellationToken
        );

        if (submissionFile == null)
        {
             _logger.LogInformation("File not Found. ID: {Id}", fileId);
            throw new HttpStatusException(HttpStatusCode.NotFound, "File not found");
        }

        bool exists = await _fileStorageService.ExistsAsync(
            submissionFile.StorageFileName,
            cancellationToken
        );

        if (!exists)
        {
             _logger.LogInformation("File not Found. StorageFileName: {StorageFileName}", submissionFile.StorageFileName);
            throw new HttpStatusException(HttpStatusCode.NotFound, "Physical file not found");
        }

        Stream stream = _fileStorageService.OpenReadAsync(
            submissionFile.StorageFileName,
            cancellationToken
        );


        return new DownloadSubmissionFileResponseModel
        {
            Stream = stream,
            ContentType = submissionFile.ContentType,
            FileName = submissionFile.OriginalFileName
        };
    }

    public async Task DeleteAsync(long fileId, CancellationToken cancellationToken = default)
    {
        SubmissionFile? file = await _context.SubmissionFiles.FirstOrDefaultAsync(
            file => file.Id == fileId,
            cancellationToken
        );

        if (file == null)
        {
             _logger.LogInformation("File not found with given ID: {Id}", fileId);
            throw new HttpStatusException(HttpStatusCode.NotFound, "File not found");
        }

        if (await _fileStorageService.ExistsAsync(file.StorageFileName, cancellationToken))
        {
            await _fileStorageService.DeleteAsync(
                file.StorageFileName,
                cancellationToken
            );
        }

        _context.SubmissionFiles.Remove(file);

        await _context.SaveChangesAsync(cancellationToken);
    }
}