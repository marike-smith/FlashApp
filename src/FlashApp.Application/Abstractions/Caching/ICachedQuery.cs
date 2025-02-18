using MediatR;

namespace FlashApp.Application.Abstractions.Caching;

public interface ICachedQuery<out TResponse> : IRequest<TResponse>, ICachedQuery;

public interface ICachedQuery
{
    string CacheKey { get; }

    TimeSpan? Expiration { get; }
}
