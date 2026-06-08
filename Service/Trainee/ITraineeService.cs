using TraineeManagement.Api.Models;
using TraineeManagement.Api.DTO.TraineeDTO;

namespace TraineeManagement.Api.Service.TraineeeInterface;

public interface ITraineeService
{
    Task<List<TraineeResponse>> GetTrainees();
    Task<List<TraineeResponse>> SearchTrainee(string search);
    Task<TraineeResponse> GetTraineeById(long id);
    Task<TraineeResponse> CreateTrainee(CreateTraineeRequest trainee); 

    Task<TraineeResponse> UpdateTrainee(UpdateTraineeRequest updatedTrainee);

    Task<bool> DeleteTrainee(long id);
}