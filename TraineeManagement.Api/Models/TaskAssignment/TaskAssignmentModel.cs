using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TraineeManagement.Api.Constants;
using TraineeManagement.Api.Enum.LearningTask;
using TraineeManagement.Api.Enum.TaskAssignment;

namespace TraineeManagement.Api.Models;

public class TaskAssignmentModel
{
    [Key]
    public long Id { get; set; }

    [Required(ErrorMessage = TaskAssignmentConstants.TraineeIdRequiredErrorMessage)]
    public long TraineeId { get; set; }

    [ForeignKey(nameof(TraineeId))]
    public TraineeModel? Trainee { get; set; } 

    [Required(ErrorMessage = TaskAssignmentConstants.MentorIdRequiredErrorMessage)]
    public long MentorId { get; set; }

    [ForeignKey(nameof(MentorId))]
    public MentorModel? Mentor { get; set; }

    [Required(ErrorMessage = TaskAssignmentConstants.LearningTaskIdRequiredErrorMessage)]
    public long LearningTaskId { get; set; }

    [ForeignKey(nameof(LearningTaskId))]
    public LearningTaskModel? LearningTask { get; set; }

    [Required(ErrorMessage = TaskAssignmentConstants.AssignedDateRequiredErrorMessage)]
    public DateTime? AssignedDate { get; set; }

    [Required(ErrorMessage = TaskAssignmentConstants.DueDateRequiredErrorMessage)]
    public DateTime? DueDate { get; set; }

    [Required(ErrorMessage = TaskAssignmentConstants.StatusRequiredErrorMessage)]
    [EnumDataType(typeof(TaskAssignmentStatusEnum), ErrorMessage = TaskAssignmentConstants.StatusValidateErrorMessage)]
    public TaskAssignmentStatusEnum? Status { get; set; }
    
    public string? Remarks { get; set; }

    public TaskAssignmentModel() { }
    public TaskAssignmentModel(long TraineeId, long MentorId, long LearningTaskId, DateTime? AssignedDate, DateTime? DueDate, TaskAssignmentStatusEnum? Status, string? Remarks)
    {
        this.TraineeId = TraineeId;
        this.MentorId = MentorId;
        this.LearningTaskId = LearningTaskId;
        this.AssignedDate = AssignedDate;
        this.DueDate = DueDate;
        this.Status = Status;
        this.Remarks = Remarks;
    }
}