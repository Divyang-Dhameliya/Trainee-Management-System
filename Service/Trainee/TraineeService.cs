using TraineeManagement.Api.Models;
using TraineeManagement.Api.Service.TraineeeInterface;
using TraineeManagement.Api.DTO.TraineeDTO;
using Microsoft.EntityFrameworkCore.Storage.Json;
using TraineeManagement.Api.Data;
using Microsoft.EntityFrameworkCore;
using TraineeManagement.Api.Constants;
namespace TraineeManagement.Api.Service.TraineeService;

public class TraineeService : ITraineeService
{
    private readonly AppDbContext _context;

    public TraineeService(AppDbContext context) { _context = context; }

    public async Task<List<TraineeResponseModel>> GetTrainees()
    {
        List<TraineeResponseModel> TraineeResponseModels = new([]);

        List<TraineeModel> trainees = await _context.Trainees.ToListAsync();

        foreach (TraineeModel t in trainees)
        {
            TraineeResponseModels.Add(
                new TraineeResponseModel(
                    t.Id,
                    t.FirstName,
                    t.LastName,
                    t.Email,
                    t.TechStack,
                    t.Status,
                    t.CreatedDate,
                    t.UpdatedDate
                )
            );
        }

        return TraineeResponseModels;
    }

    public async Task<List<TraineeResponseModel>> SearchTrainee(string search)
    {
        List<TraineeResponseModel> trainees = await _context.Trainees.Where(
            trainee =>
                trainee.FirstName != null && trainee.FirstName.Contains(search) ||
                trainee.LastName !=null && trainee.LastName.Contains(search) ||
                trainee.Email != null && trainee.Email.Contains(search) ||
                trainee.TechStack != null && trainee.TechStack.Contains(search)
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

        return trainees;
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
            _context.Trainees.ToList().Count + TraineeConstants.COUNT_INCREMENTOR_ONE,
            trainee.FirstName,
            trainee.LastName,
            trainee.Email,
            trainee.TechStack,
            trainee.Status
        );

        newTrainee.Id = _context.Trainees.ToList().Count + TraineeConstants.COUNT_INCREMENTOR_ONE;
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

    public async Task<TraineeResponseModel?> UpdateTrainee(UpdateTraineeRequestModel updatedtrainee)
    {
        TraineeModel? trainee = await _context.Trainees.FindAsync(updatedtrainee.Id);

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