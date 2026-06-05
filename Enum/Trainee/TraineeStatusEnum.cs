using System.Text.Json.Serialization;

namespace TraineeManagement.Api.Enum.Trainee;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TraineeStatus
{
    Active,
    InActive
}