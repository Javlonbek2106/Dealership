using Notification.Domain.Models;

namespace Notification.Application.Interfaces
{
    public interface ITokenService
    {
        Task<string?> CreateTokenAsync(UserCredentials credentials);

    }
}
