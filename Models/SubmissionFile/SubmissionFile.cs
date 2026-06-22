using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TraineeManagement.Api.Constants;

namespace TraineeManagement.Api.Models;

public class SubmissionFile
{
    [Key]
    public long Id { get; set; }

    [Required(ErrorMessage = SubmissionFileConstants.OriginalFileNameRequiredErrorMessage)]
    [MaxLength(SubmissionFileConstants.MaxLength, ErrorMessage = SubmissionFileConstants.OriginalFileNameMaxLengthErrorMessage)]
    public string OriginalFileName { get; set; } = null!;

    [Required(ErrorMessage = SubmissionFileConstants.StorageFileNameRequiredErrorMessage)]
    [MaxLength(SubmissionFileConstants.MaxLength, ErrorMessage = SubmissionFileConstants.StorageFileNameMaxLengthErrorMessage)]
    public string StorageFileName { get; set; } = null!;

    [Required(ErrorMessage = SubmissionFileConstants.ContentTypeRequiredErrorMessage)]
    [MaxLength(SubmissionFileConstants.ContentTypeMaxLength, ErrorMessage = SubmissionFileConstants.ContentTypeMaxLengthErrorMessage)]
    public string ContentType { get; set; } = null!;

    public long SizeInBytes { get; set; }

    [Required(ErrorMessage = SubmissionFileConstants.ChecksumRequiredErrorMessage)]
    [MaxLength(SubmissionFileConstants.ChecksumMaxLength, ErrorMessage = SubmissionFileConstants.ChecksumMaxLengthErrorMessage)]
    public string Checksum { get; set; } = null!;

    public DateTime UploadedAt { get; set; }

    public long SubmissionId { get; set; }

    [ForeignKey(nameof(SubmissionId))]
    public SubmissionModel? Submission { get; set; }
}