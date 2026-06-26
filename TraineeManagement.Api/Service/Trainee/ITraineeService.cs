using TraineeManagement.Api.DTO.TraineeDTO;
using TraineeManagement.Api.Enum.Trainee;

namespace TraineeManagement.Api.Service.TraineeeInterface;

public interface ITraineeService
{
    Task<List<TraineeResponseModel>> GetTrainees();

    Task<PaginationTraineeResponse> SearchTrainee(string search, TraineeStatus status, int pageNumber, int pageSize);

    Task<TraineeResponseModel?> GetTraineeById(long id);

    Task<TraineeResponseModel> CreateTrainee(CreateTraineeRequestModel trainee);

    Task<TraineeResponseModel?> UpdateTrainee(long id, UpdateTraineeRequestModel updatedTrainee);

    Task<bool> DeleteTrainee(long id);
}