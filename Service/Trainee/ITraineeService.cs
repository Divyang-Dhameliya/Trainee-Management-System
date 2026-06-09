using TraineeManagement.Api.Models;
using TraineeManagement.Api.DTO.TraineeDTO;

namespace TraineeManagement.Api.Service.TraineeeInterface;

public interface ITraineeService
{
    Task<List<TraineeResponseModel>> GetTrainees();

    Task<List<TraineeResponseModel>> SearchTrainee(string search);

    Task<TraineeResponseModel?> GetTraineeById(long id);

    Task<TraineeResponseModel> CreateTrainee(CreateTraineeRequestModel trainee);

    Task<TraineeResponseModel?> UpdateTrainee(UpdateTraineeRequestModel updatedTrainee);

    Task<bool> DeleteTrainee(long id);
}