using TraineeManagement.Api.Models;
using TraineeManagement.Api.Service.TraineeeInterface;
using TraineeManagement.Api.DTO.TraineeDTO;
using TraineeManagement.Api.Data;
using Microsoft.EntityFrameworkCore;
using TraineeManagement.Api.Enum.Trainee;
using TraineeManagement.Api.Service.LearningTaskInterface;
using TraineeManagement.Api.DTO.LearningTaskDTO;
using System.Net;

namespace TraineeManagement.Api.Service.LearningTaskService;

public class LearningTaskService : ILearningTaskService
{
    private readonly AppDbContext _context;
    private readonly ILogger<LearningTaskService> _logger;
    public LearningTaskService(AppDbContext context, ILogger<LearningTaskService> logger)
    { 
        _context = context; 
        _logger = logger;
    }

    public async Task<List<LearningTaskResponseModel>> GetLearningTasks()
    {
        List<LearningTaskResponseModel> LearningTaskResponseModels = new([]);

        List<LearningTaskModel> learningTasks = await _context.LearningTasks.ToListAsync();

        foreach (LearningTaskModel learningTask in learningTasks)
        {
            LearningTaskResponseModels.Add(
                new LearningTaskResponseModel(
                    learningTask.Id,
                    learningTask.Title,
                    learningTask.Description,
                    learningTask.ExpectedTechStack,
                    learningTask.DueDate,
                    learningTask.Status,
                    learningTask.CreatedDate,
                    learningTask.UpdatedDate
                )
            );
        }

        return LearningTaskResponseModels;
    }

    public async Task<LearningTaskResponseModel?> GetLearningTaskById(long id)
    {
        LearningTaskModel? learningTask = await _context.LearningTasks.FindAsync(id);

        if(learningTask == null)
        {
            _logger.LogInformation("LearningTask not found with given ID: {Id}", id);
            throw new HttpStatusException(HttpStatusCode.NotFound, "LearningTask not found with given ID.");
        }

        return new LearningTaskResponseModel(
            learningTask.Id,
            learningTask.Title,
            learningTask.Description,
            learningTask.ExpectedTechStack,
            learningTask.DueDate,
            learningTask.Status,
            learningTask.CreatedDate,
            learningTask.UpdatedDate
        );
    }

    public async Task<LearningTaskResponseModel> CreateLearningTask(CreateLearningTaskRequestModel learningTask)
    {
        LearningTaskModel newLearningTask = new LearningTaskModel(
            learningTask.Title,
            learningTask.Description,
            learningTask.ExpectedTechStack,
            learningTask.DueDate,
            learningTask.Status
        );

        _context.LearningTasks.Add(newLearningTask);
        await _context.SaveChangesAsync();

        LearningTaskResponseModel LearningTaskResponseModel = new LearningTaskResponseModel(
            newLearningTask.Id,
            newLearningTask.Title,
            newLearningTask.Description,
            newLearningTask.ExpectedTechStack,
            newLearningTask.DueDate,
            newLearningTask.Status,
            newLearningTask.CreatedDate,
            newLearningTask.UpdatedDate
        );

        return LearningTaskResponseModel;
    }

    public async Task<LearningTaskResponseModel?> UpdateLearningTask(long id, UpdateLearningTaskRequestModel updatedLearningTask)
    {
        LearningTaskModel? learningTask = await _context.LearningTasks.FindAsync(id);

        if (learningTask == null)
        {
            _logger.LogInformation("LearningTask not found with given ID: {Id}", id);
            throw new HttpStatusException(HttpStatusCode.NotFound, "LearningTask not found with given ID.");
        }

        learningTask.Title = updatedLearningTask.Title;
        learningTask.Description = updatedLearningTask.Description;
        learningTask.ExpectedTechStack = updatedLearningTask.ExpectedTechStack;
        learningTask.DueDate = updatedLearningTask.DueDate;
        learningTask.Status = updatedLearningTask.Status;
        learningTask.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new LearningTaskResponseModel(
            learningTask.Id,
            learningTask.Title,
            learningTask.Description,
            learningTask.ExpectedTechStack,
            learningTask.DueDate,
            learningTask.Status,
            learningTask.CreatedDate,
            learningTask.UpdatedDate
        );
    }

    public async Task<bool> DeleteLearningTask(long id)
    {
        LearningTaskModel? learningTask = await _context.LearningTasks.FindAsync(id);

        if (learningTask == null)
        {
            _logger.LogInformation("LearningTask not found with given ID: {Id}", id);
            throw new HttpStatusException(HttpStatusCode.NotFound, "LearningTask not found with given ID.");
        }

        _context.LearningTasks.Remove(learningTask);

        await _context.SaveChangesAsync();

        return true;
    }
}