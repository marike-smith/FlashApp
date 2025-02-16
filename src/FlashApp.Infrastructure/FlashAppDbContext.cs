using FlashApp.Application.Abstractions.DateTimeSetting;
using FlashApp.Application.Exceptions;
using FlashApp.Domain.Entities.Abstractions;
using FlashApp.Domain.Entities.Roles;
using FlashApp.Infrastructure.Outbox;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FlashApp.Infrastructure;
public sealed class FlashAppDbContext(DbContextOptions<FlashAppDbContext> options, IDateTimeProvider dateTimeProvider)
    : IdentityDbContext<ApplicationUser, Role, int>(options), IUnitOfWork
{
    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FlashAppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // First process the domain events and adding them to ChangeTracker as Outbox Messages,
            // then persisting everything in the database in a single transaction "atomic operation" 
            AddDomainEventsAsOutboxMessages();

            int result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("Concurrency exception occurred.", ex);
        }
    }

    private void AddDomainEventsAsOutboxMessages()
    {
        var outboxMessages = DomainEvents.GetDomainEvents()
            .Select(domainEvent => new OutboxMessage(
                Guid.NewGuid(),
                dateTimeProvider.UtcNow,
                domainEvent.GetType().Name,
                JsonConvert.SerializeObject(domainEvent, JsonSerializerSettings)))
            .ToList();

        DomainEvents.ClearDomainEvents();

        AddRange(outboxMessages);
    }

}
