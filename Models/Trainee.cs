using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraineeManagement.Api.Models;

public class Trainee
{
    public long Id { get; set; }

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; }
    
    [Required]
    [StringLength(50)]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string TechStack { get; set; }

    [Required]
    public string Status { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    public Trainee(long id, string firstName, string lastName, string email, string techstack, string status)
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