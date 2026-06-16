using TraineeManagement.Api.Enum.TaskAssignment;

namespace TraineeManagement.Api.DTO.TaskAssignmentDTO;

public class TaskAssignmentResponseModel
{
    public long Id { get; set; }

    public long TraineeId { get; set; }

    public long MentorId { get; set; }

    public long LearningTaskId { get; set; }

    public DateTime? AssignedDate { get; set; }

    public DateTime? DueDate { get; set; }

    public TaskAssignmentStatusEnum? Status { get; set; }
    
    public string? Remarks { get; set; }

    public TaskAssignmentResponseModel(long id, long traineeId, long mentorId, long learningTaskId, DateTime? assignedDate, DateTime? dueDate, TaskAssignmentStatusEnum? status, string? remarks)
    {
        Id = id;
        TraineeId = traineeId;
        MentorId = mentorId;
        LearningTaskId = learningTaskId;
        AssignedDate = assignedDate;
        DueDate = dueDate;
        Status = status;
        Remarks = remarks;
    }
}