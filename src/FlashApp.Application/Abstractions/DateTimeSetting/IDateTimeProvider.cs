namespace FlashApp.Application.Abstractions.DateTimeSetting;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
