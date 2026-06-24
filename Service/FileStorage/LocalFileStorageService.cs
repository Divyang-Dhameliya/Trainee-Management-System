using Microsoft.Extensions.Options;

public class LocalFileStorageService : IFileStorageService
{
    private readonly FileStorageOptions _options;

    public LocalFileStorageService(IOptions<FileStorageOptions> options)
    {
        _options = options.Value;
    }

    public Task DeleteAsync(string storageFileName, CancellationToken cancellationToken = default)
    {
        var filePath = Path.Combine(
            _options.RootPath,
            storageFileName
        );

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string storageFileName, CancellationToken cancellationToken = default)
    {
        var filePath = Path.Combine(
            _options.RootPath,
            storageFileName
        );

        return Task.FromResult(File.Exists(filePath));
    }

    public Stream OpenReadAsync(string storageFileName, CancellationToken cancellationToken = default)
    {
        var filePath = Path.Combine(
            _options.RootPath,
            storageFileName
        );

        Stream stream = new FileStream(
            filePath,
            FileMode.Open,
            FileAccess.Read
        );

        return stream;
    }

    public async Task SaveAsync(Stream stream, string storageFileName, CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(_options.RootPath);

        string filePath = Path.Combine(
            _options.RootPath,
            storageFileName
        );

        using FileStream fileStream = new FileStream(
            filePath,
            FileMode.CreateNew
        );

        await stream.CopyToAsync(
            fileStream,
            cancellationToken
        );
    }
}