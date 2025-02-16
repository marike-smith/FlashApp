using FlashApp.Application.Abstractions.Email;

namespace FlashApp.Infrastructure.Email;
internal sealed class EmailService : IEmailService
{
    public Task SendAsync(Domain.Shared.ValueObjects.Email recipient, string subject, string body)
    {
        return Task.CompletedTask;
    }
}
