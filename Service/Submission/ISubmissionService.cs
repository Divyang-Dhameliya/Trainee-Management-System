using TraineeManagement.Api.DTO.SubmissionDTO;
using TraineeManagement.Api.DTO.SubmissionFileDTO;

namespace TraineeManagement.Api.Service.SubmissionInterface;

public interface ISubmissionService
{
    Task<List<SubmissionResponseModel>> GetSubmissions();

    Task<SubmissionResponseModel?> GetSubmissionById(long id);

    Task<SubmissionResponseModel> CreateSubmission(CreateSubmissionRequestModel Submission);

    Task<SubmissionFileResponseModel> UploadAsync(long submissionId, IFormFile file, CancellationToken cancellationToken);

    Task<DownloadSubmissionFileResponseModel> DownloadAsync(long fileId, CancellationToken cancellationToken = default);

    Task DeleteAsync(long fileId, CancellationToken cancellationToken = default);
}   