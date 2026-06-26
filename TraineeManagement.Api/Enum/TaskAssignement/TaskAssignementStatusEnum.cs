using System.Text.Json.Serialization;

namespace TraineeManagement.Api.Enum.TaskAssignment;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TaskAssignmentStatusEnum
{
    Assigned,
    InProgress,
    Submitted,
    Reviewed,
    Completed

}