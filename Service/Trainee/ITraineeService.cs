using TraineeManagement.Api.Models;
using TraineeManagement.Api.DTO.TraineeDTO;

namespace TraineeManagement.Api.Service.TraineeeInterface;

public interface ITraineeService
{
    List<TraineeResponse> GetTrainees();
    TraineeResponse GetTraineeById(long id);
    TraineeResponse CreateTrainee(CreateTraineeRequest trainee); 

    TraineeResponse UpdateTrainee(UpdateTraineeRequest updatedTrainee);

    bool DeleteTrainee(long id);
}