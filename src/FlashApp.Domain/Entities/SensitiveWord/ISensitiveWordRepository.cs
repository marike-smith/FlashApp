using FlashApp.Domain.Entities.Abstractions;

namespace FlashApp.Domain.Entities.SensitiveWord
{
    public interface ISensitiveWordRepository : IRepository<SensitiveWord, Guid>
    {
    }
}
