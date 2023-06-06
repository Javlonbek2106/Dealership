using System.Security.Claims;
using Notification.Domain.Models;

namespace Notification.Application.Interfaces
{
    public interface IJwtManagerRepository
    {
        Token? GenerateAccessTokens(UserCredentials userCredentials);
        Token? GenerateRefreshToken(UserCredentials userCredentials);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
