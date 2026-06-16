using TraineeManagement.Api.DTO.SubmissionDTO;

namespace TraineeManagement.Api.Service.SubmissionInterface;

public interface ISubmissionService
{
    Task<List<SubmissionResponseModel>> GetSubmissions();

    Task<SubmissionResponseModel?> GetSubmissionById(long id);

    Task<SubmissionResponseModel> CreateSubmission(CreateSubmissionRequestModel Submission);
}