namespace TraineeManagement.Api.Constants;

public static class SubmissionFileConstants
{
    public const int MaxLength = 255;
    public const int ContentTypeMaxLength = 100;
    public const int ChecksumMaxLength = 64;
    public const string OriginalFileNameRequiredErrorMessage = "Original File name is required";
    public const string OriginalFileNameMaxLengthErrorMessage = "Original File name can't have more than 255 characters";
    public const string StorageFileNameRequiredErrorMessage = "Storage File name is required";
    public const string StorageFileNameMaxLengthErrorMessage = "Storage File name can't have more than 255 characters";
    public const string ContentTypeRequiredErrorMessage = "Content-Type is required";
    public const string ContentTypeMaxLengthErrorMessage = "Content-Type can't have more than 100 characters";
    public const string ChecksumRequiredErrorMessage = "Checksum is required";
    public const string ChecksumMaxLengthErrorMessage = "Checksum can't have more than 64 characters";
}