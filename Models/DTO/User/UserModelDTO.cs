using TraineeManagement.Api.Enum.User;

namespace TraineeManagement.Api.DTO.UserDTO;

public class UserModelDTO
{
    public long Id { get; set; }
    
    public string? UserName { get; set; }

    public UserRole? Role { get; set; }

    public UserModelDTO(long id, string? userName, UserRole? role)
    {
        Id = id;
        UserName = userName;
        Role = role;
    }
}