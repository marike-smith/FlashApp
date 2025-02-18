using FlashApp.Domain.Entities.Abstractions;

namespace FlashApp.Domain.Entities.SensitiveWord
{
    public static class UserErrors
    {
        public static readonly Error UpdateFailed = new(
        "User.UpdateFailed",
        "Unable to update user details.");

        public static readonly Error NotFound = new(
            "User.NotFound",
            "The user with the specified identifier was not found");

        public static readonly Error AddWordFailure = new(
               "AddWord.Failure",
               "An error occurred while attempting to add a new word.");

        public static readonly Error InvalidCredentials = new(
            "User.InvalidCredentials",
            "The provided credentials were invalid.");

        public static readonly Error AlreadyExists = new(
           "User.Exists",
           "The user with the specified identifier already exists.");
        
    }
}

