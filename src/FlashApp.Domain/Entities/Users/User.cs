using FlashApp.Domain.Entities.Abstractions;
using FlashApp.Domain.Entities.Users.Events;
using FlashApp.Domain.Shared.ValueObjects;

namespace FlashApp.Domain.Entities.Users
{
    public sealed class User : ApplicationUser
    {
        private User(string firstName, string lastName, string phoneNumber, Email email, Password password)
            : base(firstName, lastName, phoneNumber, email, password)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email.Value;
        }

        private User() : base() { }

        public static new User Create(string firstName, string lastName, string phoneNumber, Email email, Password password)
        {
            var User = new User(firstName, lastName, phoneNumber, email, password);

            DomainEvents.RaiseEvent(new UserCreatedDomainEvent(User.Id));

            return User;
        }
        
    }
}
