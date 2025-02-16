namespace FlashApp.Domain.Entities.Abstractions
{
    public interface IEntity
    {
        public int Id { get; }
    }

    public interface IEntity<TEntityId>
    {
        public TEntityId Id { get; }
    }

    public abstract class Entity : IEntity
    {
        public int Id { get; protected set; }
    }

    public abstract class Entity<TEntityId> : IEntity<TEntityId>
    {
        public TEntityId Id { get; protected set; }

        protected Entity(TEntityId id)
        {
            Id = id;
        }

        protected Entity() { }
    }
}