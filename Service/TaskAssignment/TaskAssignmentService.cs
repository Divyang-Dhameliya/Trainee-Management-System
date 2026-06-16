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

    public TaskAssignmentService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<TaskAssignmentResponseModel> CreateTaskAssignment(CreateTaskAssignmentRequestModel taskAssignment)
    {
        if(taskAssignment.AssignedDate > taskAssignment.DueDate)
        {
            throw new Exception("DueDate should not be before AssignedDate.");
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
        TaskAssignmentModel? taskAssignment = await _context.TaskAssignments.FindAsync(id);

        if(taskAssignment == null)
        {
            throw new HttpStatusException(HttpStatusCode.NotFound, "TaskAssignment not found with given ID.");
        }

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

    public async Task<List<TaskAssignmentResponseModel>> GetTaskAssignments()
    {
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

        return TaskAssignmentResponseModels;
    }

    public async Task<TaskAssignmentResponseModel?> UpdateTaskAssignment(long id, TaskAssignmentStatusEnum status)
    {
        TaskAssignmentModel? taskAssignment = await _context.TaskAssignments.FindAsync(id);

        if (taskAssignment == null)
        {
            throw new HttpStatusException(HttpStatusCode.NotFound, "TaskAssignment not found with given ID.");
        }

        taskAssignment.Status = status;

        await _context.SaveChangesAsync();

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