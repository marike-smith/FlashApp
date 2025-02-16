using FlashApp.Domain.Entities.Abstractions;
using MediatR;

namespace FlashApp.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
