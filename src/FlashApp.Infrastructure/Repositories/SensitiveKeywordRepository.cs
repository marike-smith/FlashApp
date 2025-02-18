using FlashApp.Application.Abstractions.Data;
using FlashApp.Domain.Entities.SensitiveWord;

namespace FlashApp.Infrastructure.Repositories;

public class SensitiveKeywordRepository(ISqlConnectionFactory sqlConnectionFactory) : Repository<SensitiveWord, Guid>(sqlConnectionFactory), ISensitiveWordRepository
{
    
}