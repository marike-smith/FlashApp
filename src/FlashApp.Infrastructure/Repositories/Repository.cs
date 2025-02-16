using FlashApp.Domain.Entities.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace FlashApp.Infrastructure.Repositories;
internal abstract class Repository<TEntity, TEntityId> : IRepository<TEntity, TEntityId>
    where TEntity : class, IEntity<TEntityId>
    where TEntityId : notnull
{
    protected readonly FlashAppDbContext DbContext;
    protected Repository(FlashAppDbContext dbContext) => DbContext = dbContext;

    public virtual async Task<TEntity> GetByIdAsync(TEntityId id, CancellationToken cancellationToken = default)
    {
        return await DbContext
            .Set<TEntity>()
            .FirstOrDefaultAsync(entity => entity.Id.Equals(id), cancellationToken);
    }

    public virtual void Add(TEntity entity) => DbContext.Add(entity);

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext
            .Set<TEntity>()
            .ToListAsync(cancellationToken);
    }

    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await DbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
    }

    public virtual void Update(TEntity entity)
    {
        DbContext.Set<TEntity>().Update(entity);
    }

    public virtual void Delete(TEntity entity)
    {
        DbContext.Set<TEntity>().Remove(entity);
    }
}
