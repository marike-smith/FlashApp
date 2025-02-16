using FlashApp.Domain.Entities.Abstractions;

namespace FlashApp.Domain.Entities.Users.Events
{
    public sealed record UserCreatedDomainEvent(int Value) : IDomainEvent
    {
    }
}