using System.Text.Json.Serialization;

namespace TraineeManagement.Api.Enum.Submission;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SubmissionStatusEnum
{
    Submitted,
    Resubmitted
}