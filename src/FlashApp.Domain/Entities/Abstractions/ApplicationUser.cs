using FlashApp.Domain.Shared.ValueObjects;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace FlashApp.Domain.Entities.Abstractions
{
    public class ApplicationUser : IdentityUser<int>, IEntity<int>, IAuditableEntity
    {
        protected ApplicationUser(string firstName, string lastName, string phoneNumber, Email email, Password password) : base()
        {
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            PhoneNumber = phoneNumber;
            Email = email.Value;
        }
        protected ApplicationUser() : base() { }

        public string IdentityId { get; protected set; } = string.Empty;

        [Required]
        public string FirstName { get; protected set; }
        [Required]
        public string LastName { get; protected set; }
        public Password Password { get; }
        public DateTime? LastLogon { get; protected set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public void SetIdentityId(int identityId) => IdentityId = identityId.ToString(CultureInfo.InvariantCulture);
 

        public static ApplicationUser Create(string firstName, string lastName, string phoneNumber, Email email, Password password)
        {
            return new ApplicationUser(firstName, lastName, phoneNumber, email, password);
        }
    }
}
