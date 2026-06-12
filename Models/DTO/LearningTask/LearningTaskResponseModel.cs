using TraineeManagement.Api.Enum.LearningTask;

namespace TraineeManagement.Api.DTO.LearningTaskDTO;

public class LearningTaskResponseModel
{
    public long Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? ExpectedTechStack { get; set; }

    public DateTime? DueDate { get; set; }

    public LearningTaskStatusEnum? Status { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public LearningTaskResponseModel(long Id, string? Title, string? Description, string? ExpectedTechStack, DateTime? DueDate, LearningTaskStatusEnum? Status, DateTime CreatedDate, DateTime UpdatedDate)
    {
        this.Id = Id;
        this.Title = Title;
        this.Description = Description;
        this.ExpectedTechStack = ExpectedTechStack;
        this.DueDate = DueDate;
        this.Status = Status;
        this.CreatedDate = CreatedDate;
        this.UpdatedDate = UpdatedDate;
    }
}