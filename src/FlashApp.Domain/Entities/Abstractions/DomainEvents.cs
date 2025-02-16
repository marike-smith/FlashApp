namespace FlashApp.Domain.Entities.Abstractions
{
    public static class DomainEvents
    {
        private static readonly List<IDomainEvent> _domainEvents = new();
        public static void ClearDomainEvents() => _domainEvents.Clear();
        public static IReadOnlyList<IDomainEvent> GetDomainEvents() => [.. _domainEvents];
        public static void RaiseEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    }
}