using Microsoft.Extensions.Caching.Distributed;

namespace FlashApp.Infrastructure.Caching;

public static class CacheOptions
{
    private static readonly DistributedCacheEntryOptions DefaultExpiration = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
    };

    public static DistributedCacheEntryOptions Create(TimeSpan? expiration = null) =>
        expiration is not null ?
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expiration } :
            DefaultExpiration;
}
