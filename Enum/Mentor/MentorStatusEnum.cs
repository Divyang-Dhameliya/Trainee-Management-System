using System.Text.Json.Serialization;

namespace TraineeManagement.Api.Enum.Mentor;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MentorStatus
{
    Active,
    InActive
}