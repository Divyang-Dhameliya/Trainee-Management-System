using TraineeManagement.Api.DTO.TaskAssignmentDTO;
using TraineeManagement.Api.Enum.TaskAssignment;

namespace TraineeManagement.Api.Service.TaskAssignmentInterface;

public interface ITaskAssignmentService
{
    Task<List<TaskAssignmentResponseModel>> GetTaskAssignments();

    Task<TaskAssignmentResponseModel?> GetTaskAssignmentById(long id);

    Task<TaskAssignmentResponseModel> CreateTaskAssignment(CreateTaskAssignmentRequestModel TaskAssignment);

    Task<TaskAssignmentResponseModel?> UpdateTaskAssignment(long id, TaskAssignmentStatusEnum status);
}