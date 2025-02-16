using FlashApp.Domain.Entities.Abstractions;

namespace FlashApp.Domain.Entities.Users
{
    public interface IUserRepository : IRepository<User, int>
    {
    }
}
