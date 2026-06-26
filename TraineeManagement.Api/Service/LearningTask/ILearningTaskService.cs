using TraineeManagement.Api.DTO.LearningTaskDTO;

namespace TraineeManagement.Api.Service.LearningTaskInterface;

public interface ILearningTaskService
{
    Task<List<LearningTaskResponseModel>> GetLearningTasks();

    Task<LearningTaskResponseModel?> GetLearningTaskById(long id);

    Task<LearningTaskResponseModel> CreateLearningTask(CreateLearningTaskRequestModel learningTask);

    Task<LearningTaskResponseModel?> UpdateLearningTask(long id, UpdateLearningTaskRequestModel updatedLearningTask);

    Task<bool> DeleteLearningTask(long id);
}