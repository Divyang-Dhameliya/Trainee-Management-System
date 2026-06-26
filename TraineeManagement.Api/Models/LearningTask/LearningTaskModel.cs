using System.ComponentModel.DataAnnotations;
using TraineeManagement.Api.Constants;
using TraineeManagement.Api.Enum.LearningTask;

namespace TraineeManagement.Api.Models;

public class LearningTaskModel
{
    [Key]
    public long Id { get; set; }

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
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    public LearningTaskModel() { }
    public LearningTaskModel(string? Title, string? Description, string? ExpectedTechStack, DateTime? DueDate, LearningTaskStatusEnum? Status)
    {
        CreatedDate = DateTime.UtcNow;
        UpdatedDate = DateTime.UtcNow;
        this.Title = Title;
        this.Description = Description;
        this.ExpectedTechStack = ExpectedTechStack;
        this.DueDate = DueDate;
        this.Status = Status;
    }
}