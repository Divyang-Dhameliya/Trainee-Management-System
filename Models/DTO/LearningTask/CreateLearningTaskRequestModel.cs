using System.ComponentModel.DataAnnotations;
using TraineeManagement.Api.Constants;
using TraineeManagement.Api.Enum.LearningTask;
using TraineeManagement.Api.Enum.Mentor;

namespace TraineeManagement.Api.DTO.LearningTaskDTO;

public class CreateLearningTaskRequestModel
{
    [Required(ErrorMessage = LearningTaskConstants.TitleRequiredErrorMessage)]
    [StringLength(LearningTaskConstants.MaxLength, ErrorMessage = LearningTaskConstants.TitleMaxLengthErrorMessage)]
    public string? Title { get; set; }

    [Required(ErrorMessage = LearningTaskConstants.DescriptionRequiredErrorMessage)]
    [StringLength(LearningTaskConstants.MaxLength, ErrorMessage = LearningTaskConstants.DescriptionMaxLengthErrorMessage)]
    public string? Description { get; set; }

    [Required(ErrorMessage = LearningTaskConstants.ExpectedTechStackRequiredErrorMessage)]
    public string? ExpectedTechStack { get; set; }

    [Required(ErrorMessage = LearningTaskConstants.DueDateRequiredErrorMessage)]
    public DateTime? DueDate { get; set; }

    [Required(ErrorMessage = LearningTaskConstants.StatusRequiredErrorMessage)]
    [EnumDataType(typeof(LearningTaskStatusEnum), ErrorMessage = LearningTaskConstants.StatusValidateErrorMessage)]
    public LearningTaskStatusEnum? Status { get; set; }

    public CreateLearningTaskRequestModel(string? Title, string? Description, string? ExpectedTechStack, DateTime? DueDate, LearningTaskStatusEnum? Status)
    {
        this.Title = Title;
        this.Description = Description;
        this.ExpectedTechStack = ExpectedTechStack;
        this.DueDate = DueDate;
        this.Status = Status;
    }
}