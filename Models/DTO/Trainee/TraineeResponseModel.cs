using System.ComponentModel.DataAnnotations;
using TraineeManagement.Api.Enum.Trainee;

namespace TraineeManagement.Api.DTO.TraineeDTO;

public class TraineeResponseModel
{
    public long Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? TechStack { get; set; }

    [EnumDataType(typeof(TraineeStatus), ErrorMessage = "Trainee status can be either Active or Inactive")]
    public TraineeStatus? Status { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public TraineeResponseModel(long id, string? firstName, string? lastName, string? email, string? techstack, TraineeStatus? status, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        CreatedDate = DateTime.UtcNow;
        UpdatedDate = DateTime.UtcNow;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        TechStack = techstack;
        Status = status;
        CreatedDate = createdAt;
        UpdatedDate = updatedAt;
    }

    public static implicit operator Task<object>(TraineeResponseModel v)
    {
        throw new NotImplementedException();
    }
}