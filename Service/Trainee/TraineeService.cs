using TraineeManagement.Api.Models;
using TraineeManagement.Api.Service.TraineeeInterface;
using TraineeManagement.Api.DTO.TraineeDTO;
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace TraineeManagement.Api.Service.TraineeService;

public class TraineeService : ITraineeService
{
    private static readonly List<Trainee> trainees = new List<Trainee>([]);

    public List<TraineeResponse> GetTrainees()
    {
        List<TraineeResponse> traineeResponses = new List<TraineeResponse> ([]);

        foreach(Trainee t in trainees)
        {
            traineeResponses.Add(new TraineeResponse(t.Id,t.FirstName,t.LastName,t.Email,t.TechStack,t.Status,t.CreatedDate,t.UpdatedDate));
        }

        return traineeResponses;
    }

    public TraineeResponse GetTraineeById(long id)
    {
        Trainee trainee = trainees.FirstOrDefault( t => t.Id == id);

        if(trainee != null){ 
            return new TraineeResponse(trainee.Id,trainee.FirstName,trainee.LastName,trainee.Email,trainee.TechStack,trainee.Status,trainee.CreatedDate,trainee.UpdatedDate);
        }

        return null;
    }

    public TraineeResponse CreateTrainee(CreateTraineeRequest trainee)
    {
        Trainee newTrainee = new Trainee(trainees.Count + 1 ,trainee.FirstName, trainee.LastName, trainee.Email, trainee.TechStack, trainee.Status);
        newTrainee.Id = trainees.Count + 1;
        trainees.Add(newTrainee);

        TraineeResponse traineeResponse = new TraineeResponse(newTrainee.Id,newTrainee.FirstName,newTrainee.LastName,newTrainee.Email,newTrainee.TechStack,newTrainee.Status,newTrainee.CreatedDate,newTrainee.UpdatedDate);
        
        return traineeResponse;
    }

    public TraineeResponse UpdateTrainee(UpdateTraineeRequest updatedtrainee)
    {
        Trainee trainee = trainees.FirstOrDefault( t => t.Id == updatedtrainee.Id);

        if(trainee == null){ 
            return null;
        }

        trainee.FirstName = updatedtrainee.FirstName;
        trainee.LastName = updatedtrainee.LastName;
        trainee.Email = updatedtrainee.Email;
        trainee.TechStack = updatedtrainee.TechStack;
        trainee.Status = updatedtrainee.Status;
        trainee.UpdatedDate = DateTime.UtcNow;

        return new TraineeResponse(trainee.Id, trainee.FirstName, trainee.LastName, trainee.Email,trainee.TechStack, trainee.Status, trainee.CreatedDate, trainee.UpdatedDate);
    }

    public bool DeleteTrainee(long id)
    {
        Trainee trainee = trainees.FirstOrDefault( t => t.Id == id);

        if(trainee == null){ 
            return false;
        }

        trainees.Remove(trainee);

        return true;
    }
}