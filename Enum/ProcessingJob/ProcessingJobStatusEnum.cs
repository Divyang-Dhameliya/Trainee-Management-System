using System.Text.Json.Serialization;

namespace TraineeManagement.Api.Enum.ProcessingJob;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ProcessingJobEnum
{
    Queued,
    Processing,
    Completed,
    Failed
}