using System.Text.Json.Serialization;

namespace TraineeManagement.Api.Enum.LearningTask;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum LearningTaskStatusEnum
{
    Draft, 
    Published,
    Closed
}