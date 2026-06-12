using TraineeManagement.Api.Enum.Mentor;

namespace TraineeManagement.Api.DTO.MentorDTO;

public class MentorResponseModel
{
    public long Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Expertise { get; set; }

    public MentorStatus? Status { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public MentorResponseModel(long Id, string? FirstName, string? LastName, string? Email, string? Expertise, MentorStatus? Status, DateTime CreatedDate, DateTime UpdatedDate)
    {
        this.Id = Id;
        this.FirstName = FirstName;
        this.LastName = LastName;
        this.Email = Email;
        this.Expertise = Expertise;
        this.Status = Status;
        this.CreatedDate = CreatedDate;
        this.UpdatedDate = UpdatedDate;
    }
}