using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TraineeManagement.Api.Constants;
using TraineeManagement.Api.Enum.LearningTask;
using TraineeManagement.Api.Enum.Mentor;
using TraineeManagement.Api.Enum.TaskAssignment;
using TraineeManagement.Api.Models;

namespace TraineeManagement.Api.DTO.TaskAssignmentDTO;

public class UpdateTaskAssignmentRequestModel
{
    [Required(ErrorMessage = TaskAssignmentConstants.TraineeIdRequiredErrorMessage)]
    public long TraineeId { get; set; }

    [Required(ErrorMessage = TaskAssignmentConstants.MentorIdRequiredErrorMessage)]
    public long MentorId { get; set; }

    [Required(ErrorMessage = TaskAssignmentConstants.LearningTaskIdRequiredErrorMessage)]
    public long LearningTaskId { get; set; }

    [Required(ErrorMessage = TaskAssignmentConstants.AssignedDateRequiredErrorMessage)]
    public DateTime? AssignedDate { get; set; }

    [Required(ErrorMessage = TaskAssignmentConstants.DueDateRequiredErrorMessage)]
    public DateTime? DueDate { get; set; }

    [Required(ErrorMessage = TaskAssignmentConstants.StatusRequiredErrorMessage)]
    [EnumDataType(typeof(TaskAssignmentStatusEnum), ErrorMessage = TaskAssignmentConstants.StatusValidateErrorMessage)]
    public TaskAssignmentStatusEnum? Status { get; set; }
    
    public string? Remarks { get; set; }

    public UpdateTaskAssignmentRequestModel(long traineeId, long mentorId, long learningTaskId, DateTime? assignedDate, DateTime? dueDate, TaskAssignmentStatusEnum? status, string? remarks)
    {
        TraineeId = traineeId;
        MentorId = mentorId;
        LearningTaskId = learningTaskId;
        AssignedDate = assignedDate;
        DueDate = dueDate;
        Status = status;
        Remarks = remarks;
    }
}