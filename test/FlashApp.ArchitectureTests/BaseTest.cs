
using System.Reflection;
using FlashApp.Application.Abstractions.Data;
using FlashApp.Domain.Entities.Abstractions;
using Microsoft.ApplicationInsights.Extensibility.Implementation;

namespace FlashApp.ArchitectureTests;

public abstract class BaseTest
{
    protected static Assembly ApplicationAssembly => typeof(ISqlConnectionFactory).Assembly;

    protected static Assembly DomainAssembly => typeof(IEntity).Assembly;

    protected static Assembly InfrastructureAssembly => typeof(UserContext).Assembly;
}
