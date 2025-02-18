using System.Data;
using Dapper;
using FlashApp.Application.Abstractions.Data;
using FlashApp.Domain.Entities.Abstractions;

namespace FlashApp.Infrastructure.Repositories;

public abstract class Repository<TEntity, TEntityId>(ISqlConnectionFactory sqlConnectionFactory) : IRepository<TEntity, TEntityId> where TEntity: Entity
{
    public virtual async Task<TEntity> GetByIdAsync(TEntityId id, CancellationToken cancellationToken = default)
    {
        using var connection = sqlConnectionFactory.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<TEntity>(
            "sp_GetCompanyById",
            new { Id = id },
            commandType: CommandType.StoredProcedure
        );
    }

    public virtual void Add(TEntity entity) => throw new NotImplementedException();
    
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
    
    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
    
    public virtual void Update(TEntity entity)
    {
        throw new NotImplementedException();
    }
    
    public virtual void Delete(TEntity entity)
    {
       throw new NotImplementedException();
    }
}
