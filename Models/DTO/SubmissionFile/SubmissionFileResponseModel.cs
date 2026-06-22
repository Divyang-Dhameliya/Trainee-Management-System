namespace TraineeManagement.Api.DTO.SubmissionFileDTO;

public class SubmissionFileResponseModel
{
    public long Id { get; set; }

    public string OriginalFileName { get; set; } = null!;

    public string ContentType { get; set; } = null!;

    public long SizeInBytes { get; set; }

    public DateTime UploadedAt { get; set; }
}