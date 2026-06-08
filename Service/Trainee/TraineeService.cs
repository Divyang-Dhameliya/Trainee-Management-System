using TraineeManagement.Api.Models;
using TraineeManagement.Api.Service.TraineeeInterface;
using TraineeManagement.Api.DTO.TraineeDTO;
using Microsoft.EntityFrameworkCore.Storage.Json;
using TraineeManagement.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace TraineeManagement.Api.Service.TraineeService;

public class TraineeService : ITraineeService
{
    private static readonly List<Trainee> trainees = new List<Trainee>([]);
    private readonly AppDbContext _context;

    public TraineeService(AppDbContext context){ _context = context; }

    public async Task<List<TraineeResponse>> GetTrainees()
    {
        List<TraineeResponse> traineeResponses = new List<TraineeResponse> ([]);

        List<Trainee> trainees = await _context.Trainees.ToListAsync();

        foreach(Trainee t in trainees)
        {
            traineeResponses.Add(new TraineeResponse(t.Id,t.FirstName,t.LastName,t.Email,t.TechStack,t.Status,t.CreatedDate,t.UpdatedDate));
        }

        return traineeResponses;
    }

    public async Task<List<TraineeResponse>> SearchTrainee(string search)
    {
        var trainees = await _context.Trainees.Where(
            trainee =>
                trainee.FirstName.Contains(search) ||
                trainee.LastName.Contains(search) ||
                trainee.Email.Contains(search) ||
                trainee.TechStack.Contains(search)
        ).Select(
            trainee => new TraineeResponse(trainee.Id,trainee.FirstName,trainee.LastName,trainee.Email,trainee.TechStack,trainee.Status,trainee.CreatedDate,trainee.UpdatedDate)
        ).ToListAsync();

        return trainees;
    }

    public async Task<TraineeResponse> GetTraineeById(long id)
    {
        Trainee trainee = await _context.Trainees.FindAsync(id);

        if(trainee != null){ 
            return new TraineeResponse(trainee.Id,trainee.FirstName,trainee.LastName,trainee.Email,trainee.TechStack,trainee.Status,trainee.CreatedDate,trainee.UpdatedDate);
        }

        return null;
    }

    public async Task<TraineeResponse> CreateTrainee(CreateTraineeRequest trainee)
    {

        Trainee newTrainee = new Trainee(_context.Trainees.ToList().Count + 1 ,trainee.FirstName, trainee.LastName, trainee.Email, trainee.TechStack, trainee.Status);
        newTrainee.Id = _context.Trainees.ToList().Count + 1;
        _context.Trainees.Add(newTrainee);
        await _context.SaveChangesAsync();

        TraineeResponse traineeResponse = new TraineeResponse(newTrainee.Id,newTrainee.FirstName,newTrainee.LastName,newTrainee.Email,newTrainee.TechStack,newTrainee.Status,newTrainee.CreatedDate,newTrainee.UpdatedDate);
        
        return traineeResponse;
    }

    public async Task<TraineeResponse> UpdateTrainee(UpdateTraineeRequest updatedtrainee)
    {
        Trainee trainee = await _context.Trainees.FindAsync(updatedtrainee.Id);

        if(trainee == null){ 
            return null;
        }

        trainee.FirstName = updatedtrainee.FirstName;
        trainee.LastName = updatedtrainee.LastName;
        trainee.Email = updatedtrainee.Email;
        trainee.TechStack = updatedtrainee.TechStack;
        trainee.Status = updatedtrainee.Status;
        trainee.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new TraineeResponse(trainee.Id, trainee.FirstName, trainee.LastName, trainee.Email,trainee.TechStack, trainee.Status, trainee.CreatedDate, trainee.UpdatedDate);
    }

    public async Task<bool> DeleteTrainee(long id)
    {
        Trainee trainee = await _context.Trainees.FindAsync(id);

        if(trainee == null){ 
            return false;
        }

        _context.Trainees.Remove(trainee);

        await _context.SaveChangesAsync();

        return true;
    }
}