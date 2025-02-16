using FlashApp.Domain.Entities.Abstractions;
using MediatR;

namespace FlashApp.Application.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
