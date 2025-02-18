using FlashApp.Domain.Entities.Abstractions;

namespace FlashApp.Domain.Entities.SensitiveWord.Events
{
    public sealed record SensitiveWordCreatedDomainEvent(Guid Value) : IDomainEvent
    {
    }
}