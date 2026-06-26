using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using TraineeManagement.Api.Data;
using TraineeManagement.Api.DTO.TaskAssignmentDTO;
using TraineeManagement.Api.Enum.TaskAssignment;
using TraineeManagement.Api.Models;
using TraineeManagement.Api.Service.TaskAssignmentInterface;

public class TaskAssignmentService : ITaskAssignmentService
{
    private readonly AppDbContext _context;
    private readonly ILogger<TaskAssignmentService> _logger;
    private readonly ICacheService _cacheService; 

    public TaskAssignmentService(AppDbContext context, ILogger<TaskAssignmentService> logger, ICacheService cacheService)
    {
        _context = context;
        _logger = logger;
        _cacheService = cacheService;
    }

    public async Task<TaskAssignmentResponseModel> CreateTaskAssignment(CreateTaskAssignmentRequestModel taskAssignment)
    {
        if(taskAssignment.AssignedDate > taskAssignment.DueDate)
        {
            _logger.LogInformation("DueDate should not be before AssignedDate. DueDate: {DueDate} AssinedDate: {AssignedDate}", taskAssignment.DueDate, taskAssignment.AssignedDate);
            throw new HttpStatusException(HttpStatusCode.BadRequest, "DueDate should not be before AssignedDate.");
        }

        TaskAssignmentModel newTaskAssignment = new TaskAssignmentModel(
            taskAssignment.TraineeId,
            taskAssignment.MentorId,
            taskAssignment.LearningTaskId,
            taskAssignment.AssignedDate,
            taskAssignment.DueDate,
            taskAssignment.Status,
            taskAssignment.Remarks
        );

        _context.TaskAssignments.Add(newTaskAssignment);
        await _context.SaveChangesAsync();

        await _cacheService.RemoveAsync(CacheKeys.TaskAssignmentsAll);

        TaskAssignmentResponseModel TaskAssignmentResponseModel = new TaskAssignmentResponseModel(
            newTaskAssignment.Id,
            newTaskAssignment.TraineeId,
            newTaskAssignment.MentorId,
            newTaskAssignment.LearningTaskId,
            newTaskAssignment.AssignedDate,
            newTaskAssignment.DueDate,
            newTaskAssignment.Status,
            newTaskAssignment.Remarks
        );

        return TaskAssignmentResponseModel;
    }

    public async Task<TaskAssignmentResponseModel?> GetTaskAssignmentById(long id)
    {
        string cacheKey = CacheKeys.TaskAssignment(id);

        TaskAssignmentResponseModel? cachedTaskAssignment = await _cacheService.GetAsync<TaskAssignmentResponseModel>(cacheKey);
        
        if(cachedTaskAssignment != null)
        {
            _logger.LogInformation("Cache hit for {CacheKey}", cacheKey);
            return cachedTaskAssignment;
        }

        _logger.LogInformation("Cache miss for {CacheKey}", cacheKey);

        TaskAssignmentModel? taskAssignment = await _context.TaskAssignments.FindAsync(id);

        if(taskAssignment == null)
        {
            _logger.LogInformation("TaskAssignment not found with given ID: {Id}", id);
            throw new HttpStatusException(HttpStatusCode.NotFound, "TaskAssignment not found with given ID.");
        }

        TaskAssignmentResponseModel response = new TaskAssignmentResponseModel(
            taskAssignment.Id,
            taskAssignment.TraineeId,
            taskAssignment.MentorId,
            taskAssignment.LearningTaskId,
            taskAssignment.AssignedDate,
            taskAssignment.DueDate,
            taskAssignment.Status,
            taskAssignment.Remarks
        );

        await _cacheService.SetAsync(cacheKey, response, TimeSpan.FromMinutes(10));
        
        return response;
    }

    public async Task<List<TaskAssignmentResponseModel>> GetTaskAssignments()
    {
        string cacheKey = CacheKeys.TaskAssignmentsAll;

        List<TaskAssignmentResponseModel>? cachedTaskAssignments = await _cacheService.GetAsync<List<TaskAssignmentResponseModel>>(cacheKey);
        
        if(cachedTaskAssignments != null)
        {
            _logger.LogInformation("Cache hit for {CacheKey}", cacheKey);
            return cachedTaskAssignments;
        }

        _logger.LogInformation("Cache miss for {CacheKey}", cacheKey);

        List<TaskAssignmentResponseModel> TaskAssignmentResponseModels = new([]);

        List<TaskAssignmentModel> taskAssignments = await _context.TaskAssignments.ToListAsync();

        foreach (TaskAssignmentModel taskAssignment in taskAssignments)
        {
            TaskAssignmentResponseModels.Add(
                new TaskAssignmentResponseModel(
                    taskAssignment.Id,
                    taskAssignment.TraineeId,
                    taskAssignment.MentorId,
                    taskAssignment.LearningTaskId,
                    taskAssignment.AssignedDate,
                    taskAssignment.DueDate,
                    taskAssignment.Status,
                    taskAssignment.Remarks
                )
            );
        }

        await _cacheService.SetAsync(cacheKey, TaskAssignmentResponseModels, TimeSpan.FromMinutes(10));

        return TaskAssignmentResponseModels;
    }

    public async Task<TaskAssignmentResponseModel?> UpdateTaskAssignment(long id, TaskAssignmentStatusEnum status)
    {
        TaskAssignmentModel? taskAssignment = await _context.TaskAssignments.FindAsync(id);

        if (taskAssignment == null)
        {
            _logger.LogInformation("TaskAssignment not found with given ID: {Id}", id);
            throw new HttpStatusException(HttpStatusCode.NotFound, "TaskAssignment not found with given ID.");
        }

        taskAssignment.Status = status;

        await _context.SaveChangesAsync();

        await _cacheService.RemoveAsync(CacheKeys.TaskAssignment(id));
        await _cacheService.RemoveAsync(CacheKeys.TaskAssignmentsAll);

        return new TaskAssignmentResponseModel(
            taskAssignment.Id,
            taskAssignment.TraineeId,
            taskAssignment.MentorId,
            taskAssignment.LearningTaskId,
            taskAssignment.AssignedDate,
            taskAssignment.DueDate,
            taskAssignment.Status,
            taskAssignment.Remarks
        );
    }
}