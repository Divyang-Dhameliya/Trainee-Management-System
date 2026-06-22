public interface IFileStorageService
{
    Task SaveAsync(Stream stream, string storageFileName, CancellationToken cancellationToken = default);

    Stream OpenReadAsync(string storageFileName, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(string storageFileName, CancellationToken cancellationToken = default);

    Task DeleteAsync(string storageFileName, CancellationToken cancellationToken = default);
}