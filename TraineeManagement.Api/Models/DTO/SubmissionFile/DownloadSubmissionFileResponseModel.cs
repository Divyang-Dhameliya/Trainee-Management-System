namespace TraineeManagement.Api.DTO.SubmissionFileDTO;

public class DownloadSubmissionFileResponseModel
{
    public Stream Stream { get; set; } = null!;

    public string ContentType { get; set; } = null!;

    public string FileName { get; set; } = null!;
}