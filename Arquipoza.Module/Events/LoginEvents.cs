using Microsoft.Extensions.Logging;
using OrchardCore.Users;
using OrchardCore.Users.Events;
using OrchardCore.Users.Models;

namespace Arquipoza.Module.Events
{
    public class LoginEvents : ILoginFormEvent
    {
        private readonly ILogger<LoginEvents> _logger;

        public LoginEvents(ILogger<LoginEvents> logger)
        {
            _logger = logger;
        }

        public Task LoggedInAsync(IUser user)
        {
            _logger.LogInformation("User {UserName} logged in to Arquipoza.", user.UserName);
            return Task.CompletedTask;
        }

        public Task LoggingInAsync(string userName, Action<string, string> reportError)
        {
            return Task.CompletedTask;
        }

        public Task LoggingInFailedAsync(string userName)
        {
            _logger.LogWarning("Failed login attempt for {UserName}.", userName);
            return Task.CompletedTask;
        }

        public Task LoggingInFailedAsync(IUser user)
        {
            _logger.LogWarning("Failed login attempt for user {UserName}.", user.UserName);
            return Task.CompletedTask;
        }

        public Task IsLockedOutAsync(IUser user)
        {
            return Task.CompletedTask;
        }
    }
}