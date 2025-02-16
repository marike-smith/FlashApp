namespace FlashApp.Infrastructure.Outbox;
public sealed class OutboxMessage(Guid id, DateTime ocurredOnUtc, string type, string content)
{
    public Guid Id { get; private set; } = id;
    public DateTime OcurredOnUtc { get; private set; } = ocurredOnUtc;

    /// <summary>
    /// Name of the Domain Event
    /// </summary>
    public string Type { get; private set; } = type;

    /// <summary>
    /// JSON string containing the serialized domain event
    /// </summary>
    public string Content { get; private set; } = content;
    public DateTime? ProcessedOnUtc { get; private set; }
    public string Error { get; private set; }
}
