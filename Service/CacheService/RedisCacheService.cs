using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

public class RedisCacheService : ICacheService
{
    private readonly ILogger<RedisCacheService> _logger;
    private readonly IDistributedCache _cache;

    public RedisCacheService(IDistributedCache cache, ILogger<RedisCacheService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            dynamic? json = await _cache.GetStringAsync(
                key,
                cancellationToken
            );

            if(json == null)
                return default;
            
            return JsonSerializer.Deserialize<T>(json);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Redis get failed for key {Key}", key);
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan expiration, CancellationToken cancellationToken = default)
    {
        try
        {
            var json = JsonSerializer.Serialize(value);

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };

            await _cache.SetStringAsync(
                key,
                json,
                options,
                cancellationToken
            );
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Redis get failed for key {Key}", key);
        }
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            await _cache.RemoveAsync(
                key,
                cancellationToken
            );
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Redis remove failed for key {Key}", key);
        }
    }
}