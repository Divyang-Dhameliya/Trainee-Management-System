using TraineeManagement.Api.Models;
using TraineeManagement.Api.Service.TraineeeInterface;
using TraineeManagement.Api.DTO.TraineeDTO;
using TraineeManagement.Api.Data;
using Microsoft.EntityFrameworkCore;
using TraineeManagement.Api.Enum.Trainee;

namespace TraineeManagement.Api.Service.TraineeService;

public class TraineeService : ITraineeService
{
    private readonly AppDbContext _context;

    public TraineeService(AppDbContext context)
    { 
        _context = context; 
    }

    public async Task<List<TraineeResponseModel>> GetTrainees()
    {
        List<TraineeResponseModel> TraineeResponseModels = new([]);

        List<TraineeModel> trainees = await _context.Trainees.ToListAsync();

        foreach (TraineeModel trainee in trainees)
        {
            TraineeResponseModels.Add(
                new TraineeResponseModel(
                    trainee.Id,
                    trainee.FirstName,
                    trainee.LastName,
                    trainee.Email,
                    trainee.TechStack,
                    trainee.Status,
                    trainee.CreatedDate,
                    trainee.UpdatedDate
                )
            );
        }

        return TraineeResponseModels;
    }

    public async Task<PaginationTraineeResponse> SearchTrainee(string search, TraineeStatus status, int pageNumber, int pageSize)
    {
        List<TraineeResponseModel> trainees = await _context.Trainees.Where(
            trainee =>
                (trainee.FirstName != null && trainee.FirstName.Contains(search) ||
                trainee.LastName !=null && trainee.LastName.Contains(search) ||
                trainee.Email != null && trainee.Email.Contains(search) ||
                trainee.TechStack != null && trainee.TechStack.Contains(search)) &&
                trainee.Status == status
        ).Select(
            trainee => new TraineeResponseModel(
                trainee.Id,
                trainee.FirstName,
                trainee.LastName,
                trainee.Email,
                trainee.TechStack,
                trainee.Status,
                trainee.CreatedDate,
                trainee.UpdatedDate
            )
        ).ToListAsync();


        IEnumerable<TraineeResponseModel> traineeRes = trainees.Skip(pageSize*(pageNumber-1)).Take(pageSize);

        return new PaginationTraineeResponse(pageNumber, pageSize, trainees.Count, traineeRes);
    }

    public async Task<TraineeResponseModel?> GetTraineeById(long id)
    {
        TraineeModel? trainee = await _context.Trainees.FindAsync(id);

        if (trainee != null)
        {
            return new TraineeResponseModel(
                trainee.Id,
                trainee.FirstName,
                trainee.LastName,
                trainee.Email,
                trainee.TechStack,
                trainee.Status,
                trainee.CreatedDate,
                trainee.UpdatedDate
            );
        }

        return null;
    }

    public async Task<TraineeResponseModel> CreateTrainee(CreateTraineeRequestModel trainee)
    {
        TraineeModel newTrainee = new TraineeModel(
            trainee.FirstName,
            trainee.LastName,
            trainee.Email,
            trainee.TechStack,
            trainee.Status
        );

        _context.Trainees.Add(newTrainee);
        await _context.SaveChangesAsync();

        TraineeResponseModel TraineeResponseModel = new TraineeResponseModel(
            newTrainee.Id,
            newTrainee.FirstName,
            newTrainee.LastName,
            newTrainee.Email,
            newTrainee.TechStack,
            newTrainee.Status,
            newTrainee.CreatedDate,
            newTrainee.UpdatedDate
        );

        return TraineeResponseModel;
    }

public async Task<TraineeResponseModel?> UpdateTrainee(long id, UpdateTraineeRequestModel updatedtrainee)
    {
        TraineeModel? trainee = await _context.Trainees.FindAsync(id);

        if (trainee == null)
        {
            return null;
        }

        trainee.FirstName = updatedtrainee.FirstName;
        trainee.LastName = updatedtrainee.LastName;
        trainee.Email = updatedtrainee.Email;
        trainee.TechStack = updatedtrainee.TechStack;
        trainee.Status = updatedtrainee.Status;
        trainee.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new TraineeResponseModel(
            trainee.Id,
            trainee.FirstName,
            trainee.LastName,
            trainee.Email,
            trainee.TechStack,
            trainee.Status,
            trainee.CreatedDate,
            trainee.UpdatedDate
        );
    }

    public async Task<bool> DeleteTrainee(long id)
    {
        TraineeModel? trainee = await _context.Trainees.FindAsync(id);

        if (trainee == null)
        {
            return false;
        }

        _context.Trainees.Remove(trainee);

        await _context.SaveChangesAsync();

        return true;
    }
}