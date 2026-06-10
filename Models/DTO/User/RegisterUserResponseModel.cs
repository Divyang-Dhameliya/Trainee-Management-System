using TraineeManagement.Api.Enum.User;

namespace TraineeManagement.Api.DTO.UserDTO;

public class RegisterUserResponseModel
{
    public string? UserName { get; set; }

    public string? Email { get; set; }

    public UserRole? Role { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public RegisterUserResponseModel(string? userName, string? email, UserRole? role, DateTime createdDate, DateTime updatedDate)
    {
        UserName = userName;
        Email = email;
        Role = role;
        CreatedDate = createdDate;
        UpdatedDate = updatedDate;
    }
}