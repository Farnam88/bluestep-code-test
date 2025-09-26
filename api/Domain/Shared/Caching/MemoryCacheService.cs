using Microsoft.Extensions.Caching.Memory;

namespace api.Domain.Shared.Caching;

/// <summary>
/// Simple caching service
/// </summary>
public interface IMemoryCacheService
{
    /// <summary>
    /// Adds an object to the cache with a specified key and expiration time.
    /// </summary>
    /// <param name="key">The key to store the data with.</param>
    /// <param name="value">The data to be cached.</param>
    /// <param name="absoluteExpirationRelativeToNow">The expiration timespan.</param>
    void Add<T>(string key, T value, TimeSpan? absoluteExpirationRelativeToNow = null);

    /// <summary>
    /// Retrieves an object from the cache by its key.
    /// </summary>
    /// <param name="key">The key of the object to retrieve.</param>
    /// <param name="value">returned value could be null if it does not exist</param>
    /// <typeparam name="T">expected object to be returned</typeparam>
    /// <returns>The cached object, or null if the key is not found.</returns>
    bool TryGet<T>(string key, out T? value);

    /// <summary>
    /// Removes an object from the cache by its key.
    /// </summary>
    /// <param name="key">The key of the object to remove.</param>
    void Remove(string key);
}

/// <inheritdoc cref="IMemoryCacheService" />
public class MemoryCacheService : IMemoryCacheService
{
    private readonly IMemoryCache _cache;

    /// <summary>
    /// Initializes a new instance of the <see cref="MemoryCacheService"/> class.
    /// </summary>
    public MemoryCacheService()
    {
        _cache = new MemoryCache(new MemoryCacheOptions());
    }

    /// <inheritdoc />
    public void Add<T>(string key, T value, TimeSpan? absoluteExpirationRelativeToNow = null)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key), "Cache key cannot be null or empty.");
        }

        if (value == null)
        {
            throw new ArgumentNullException(nameof(value), "Cache value cannot be null.");
        }

        var cacheEntryOptions = new MemoryCacheEntryOptions();

        if (absoluteExpirationRelativeToNow.HasValue)
        {
            cacheEntryOptions.AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow;
        }
        else
        {
            // Default expiration if none is provided (e.g., 1 hour)
            cacheEntryOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
        }

        _cache.Set(key, value, cacheEntryOptions);
    }

    /// <inheritdoc />
    public bool TryGet<T>(string key, out T? value)
    {
        if (_cache.TryGetValue(key, out value))
            return true;
        return false;
    }

    /// <inheritdoc />
    public void Remove(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key), "Cache key cannot be null or empty.");
        }

        _cache.Remove(key);
    }
}