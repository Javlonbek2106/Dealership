using Microsoft.EntityFrameworkCore;
using Notification.Application.Interfaces;
using Notification.Domain.Models;

namespace Infrastructure.Services.JWTService
{
    public class UserRefreshTokenService : IUserRefreshTokenService
    {
        private readonly IDealershipDbContext _dbContext;
        private readonly IJwtManagerRepository _jwtManagerRepository;

        public UserRefreshTokenService(IDealershipDbContext dbContext, IJwtManagerRepository jwtManagerRepository)
        {
            this._dbContext = dbContext;
            this._jwtManagerRepository = jwtManagerRepository;
        }

        public async Task<UserRefreshTokens> AddUserRefreshTokens(UserRefreshTokens user)
        {
            int count = _dbContext.UserRefreshToken.ToList().Count;
            user.Id = count + 1;
            _dbContext.UserRefreshToken.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserRefreshTokens(UserCredentials credentials, string refreshToken)
        {
            var userRefreshTokens = _dbContext.UserRefreshToken.Include(x => x.Employee)
                .FirstOrDefault(x => x.Employee.Username == credentials.UserName &&
                                     x.Employee.Email == credentials.EmailAddress &&
                                     x.Employee.Password == credentials.Password &&
                                     x.RefreshToken == refreshToken);

            if (userRefreshTokens is null) return false;

            _dbContext.UserRefreshToken.Remove(userRefreshTokens);
            await _dbContext.SaveChangesAsync();
            return true;

        }

        public Task<UserRefreshTokens?> GetSavedRefreshTokens(UserCredentials credentials, string refreshToken)
        {
            UserRefreshTokens? userRefreshTokens = _dbContext.UserRefreshToken.Include(x => x.Employee).Select(x => x)
                .FirstOrDefault(x => x.Employee.Username == credentials.UserName &&
                                     x.Employee.Email == credentials.EmailAddress &&
                                     x.Employee.Password == credentials.Password &&
                                     x.RefreshToken == refreshToken);


            return Task.FromResult(userRefreshTokens);
        }
    }
}
