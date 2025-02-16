namespace FlashApp.Application.Abstractions.Authentication;
public interface IUserContext
{
    int Id { get; }

    string IdentityId { get; }
}
