
using System.Reflection;
using FlashApp.Application.Abstractions.Messaging;
using FlashApp.Domain.Entities.Abstractions;
using FlashApp.Infrastructure;

namespace FlashApp.ArchitectureTests;

public abstract class BaseTest
{
    protected static Assembly ApplicationAssembly => typeof(IBaseCommand).Assembly;

    protected static Assembly DomainAssembly => typeof(IEntity).Assembly;

    protected static Assembly InfrastructureAssembly => typeof(FlashAppDbContext).Assembly;
}
