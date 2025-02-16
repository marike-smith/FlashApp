using FlashApp.Domain.Entities.Abstractions;

namespace FlashApp.Domain.UnitTests;

public abstract class BaseTest
{
    public static T AssertDomainEventWasPublished<T, TId>(IEntity<TId> entity)
        where T : IDomainEvent
    {
        T? domainEvent = DomainEvents.GetDomainEvents().OfType<T>().SingleOrDefault() ?? throw new Exception($"{typeof(T).Name} was not published");

        return domainEvent;
    }
}
