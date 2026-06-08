using System.ComponentModel.DataAnnotations;
using TraineeManagement.Api.Enum.Trainee;
namespace TraineeManagement.Api.Models;

public class Trainee
{
    public long Id { get; set; }

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

    [Required]
    [EnumDataType(typeof(TraineeStatus),ErrorMessage = "Trainee status can be either Active or Inactive")]
    public TraineeStatus Status { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    public Trainee() {}
    public Trainee(long id, string firstName, string lastName, string email, string techstack, TraineeStatus status)
    {
        Id = id;
        CreatedDate = DateTime.UtcNow;
        UpdatedDate = DateTime.UtcNow;      
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        TechStack = techstack;
        Status = status;
    }
}