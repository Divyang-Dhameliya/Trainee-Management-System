using System.ComponentModel.DataAnnotations;
using TraineeManagement.Api.Enum.Trainee;

namespace TraineeManagement.Api.DTO.TraineeDTO;

public class CreateTraineeRequest
{
    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, ErrorMessage = "Firstname can't have more than 50 characters")]
    public string FirstName { get; set; }
    
    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, ErrorMessage = "Lastname can't have more than 50 characters")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Valid email is required")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Tech-stack is required")]
    public string TechStack { get; set; }

    [Required(ErrorMessage = "Status is required")]
    [EnumDataType(typeof(TraineeStatus),ErrorMessage = "Trainee status can be either Active or Inactive")]
    public TraineeStatus Status { get; set; }

    public CreateTraineeRequest(string firstName, string lastName, string email, string techstack, TraineeStatus status)
    {     
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        TechStack = techstack;
        Status = status;
    }
}