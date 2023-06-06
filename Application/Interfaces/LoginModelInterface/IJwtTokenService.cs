using System.Security.Claims;
using Application.Models.UserCredentials;
using Domain.Entities.IdentityEntities.Token;

namespace Application.ServiceInterfaces.LoginModelInterface
{
    public interface IJwtTokenService
    {

        public Tokens CreateToken(UserCredentials user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
