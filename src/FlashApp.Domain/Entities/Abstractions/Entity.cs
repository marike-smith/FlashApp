namespace FlashApp.Domain.Entities.Abstractions
{
    public interface IEntity
    {
        public Guid Id { get; }
    }

    public abstract class Entity : IEntity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
    }
}