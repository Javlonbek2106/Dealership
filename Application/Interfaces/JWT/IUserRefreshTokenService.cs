using Notification.Domain.Models;

namespace Notification.Application.Interfaces
{
    public interface IUserRefreshTokenService
    {
        Task<UserRefreshTokens?> AddUserRefreshTokens(UserRefreshTokens user);
        Task<UserRefreshTokens?> GetSavedRefreshTokens(UserCredentials credentials, string refreshtoken);
        Task<bool> DeleteUserRefreshTokens(UserCredentials credentials, string refreshToken);
    }
}
