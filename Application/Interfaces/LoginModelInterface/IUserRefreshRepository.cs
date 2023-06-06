using Domain.Entities.IdentityEntities;

namespace Application.Interfaces.LoginModelInterface
{
    public interface IUserRefreshRepository
    {
        RefreshTokens AddUserRefreshTokens(RefreshTokens user);
        Task<RefreshTokens> UpdateUserRefreshTokens(RefreshTokens user);
        RefreshTokens GetSavedRefreshTokens(string refreshToken);
        Task<int> SaveCommit();
        List<RefreshTokens> GetAllUserRefreshTokens();
        RefreshTokens GetUserRefreshTokensById(Guid id);
        Task<RefreshTokens> DeleteUserRefreshTokens(RefreshTokens userRefreshTokens);
    }
}
