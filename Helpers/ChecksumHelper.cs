using System.Security.Cryptography;

namespace TraineeManagement.Api.Helpers;

public static class ChecksumHelper
{
    public async static Task<string> ComputeAsync(Stream stream, CancellationToken cancellationToken)
    {
        using SHA256 sha256 = SHA256.Create();

        byte[] hash = await sha256.ComputeHashAsync(
            stream,
            cancellationToken
        );

        return Convert.ToHexString(hash);
    }    
}