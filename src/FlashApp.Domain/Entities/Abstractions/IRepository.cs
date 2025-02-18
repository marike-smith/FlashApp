namespace FlashApp.Domain.Entities.Abstractions
{
    public interface IRepository<TEntity, in TEntityId>
            where TEntity : Entity
            where TEntityId : notnull
    {
        Task<TEntity> GetByIdAsync(TEntityId id, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
