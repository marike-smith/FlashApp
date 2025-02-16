using FlashApp.Domain.Entities.Abstractions;
using FlashApp.Domain.Entities.Users;

namespace FlashApp.Infrastructure.Repositories
{
    internal sealed class UserRepository : Repository<ApplicationUser, int>, IUserRepository
    {
        public UserRepository(FlashAppDbContext dbContext) : base(dbContext)
        {
        }

        public Task<User> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(User entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Add(User entity)
        {
            throw new NotImplementedException();
        }

        public void Update(User entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
