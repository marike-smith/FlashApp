using FlashApp.Domain.Entities.Abstractions;
using FlashApp.Domain.Shared.ValueObjects;
using MediatR;

namespace FlashApp.Application.SensitiveWords.Add;
public sealed record AddWordRequest(Word Word) : IRequest<Result<AddWordResponse>>;
