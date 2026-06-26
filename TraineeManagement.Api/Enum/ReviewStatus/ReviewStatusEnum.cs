using System.Text.Json.Serialization;

namespace TraineeManagement.Api.Enum.Review;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ReviewStatusEnum
{
    Accepted,
    ChangesRequired,
    Rejected
}