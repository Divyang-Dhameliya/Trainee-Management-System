using System.Text.Json.Serialization;

namespace TraineeManagement.Api.Enum.User;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserRole
{
    Admin, 
    Mentor, 
    Trainee
}