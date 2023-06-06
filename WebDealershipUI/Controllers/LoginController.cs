using Application.Abstraction;
using Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notification.Application.Interfaces;
using Notification.Domain.Models;

namespace Notification.UI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IDealershipDbContext _dbContext;
        private readonly ITokenService _tokenService;
        private readonly IJwtManagerRepository _jwtManagerRepository;
        private readonly IUserRefreshTokenService _userRefreshTokenService;

        private readonly IConfiguration _configuration;

        public LoginController
            (
                    IDealershipDbContext dbContext,
                    ITokenService tokenService,
                    IConfiguration configuration,
                    IJwtManagerRepository jwtManagerRepository,
                    IUserRefreshTokenService userRefreshTokenService
            )
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
            _configuration = configuration;
            _jwtManagerRepository = jwtManagerRepository;
            _userRefreshTokenService = userRefreshTokenService;
        }


        [HttpPost("login")]
        public async Task<IResult> Login([FromBody] UserCredentials credentials)
        {
            Token? token = _jwtManagerRepository.GenerateAccessTokens(credentials);

            if (token is null)
                return Results.NotFound("User not found");

            Employee? user = _dbContext.Employees.FirstOrDefault(x => x.Username == credentials.UserName &&
                                                         x.Password == credentials.Password.ComputeSha256Hash() &&
                                                         x.Email == credentials.EmailAddress);

            if (user is null)
                return Results.NotFound("User not found");
            int.TryParse(_configuration["Jwt:AccessTokenLifetime"], out int lifetime);

            UserRefreshTokens obj = new UserRefreshTokens()
            {
                Employee = user,
                RefreshToken = token.RefreshToken,
                ExpirationDate = DateTimeOffset.UtcNow.AddMinutes(lifetime),
            };

            UserRefreshTokens? userRefreshTokens = _dbContext.UserRefreshToken
                                                        .FirstOrDefault(o => o.Employee.Id == user.Id);

            if (userRefreshTokens is null)
            {
                await _userRefreshTokenService.AddUserRefreshTokens(obj);
            }
            else
            {
                _dbContext.UserRefreshToken.Remove(userRefreshTokens);
                await _dbContext.SaveChangesAsync();
                await _userRefreshTokenService.AddUserRefreshTokens(obj);
            }

            return Results.Ok(token);


        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] Token token)
        {
            var principal = _jwtManagerRepository
                            .GetPrincipalFromExpiredToken(token.AccessToken);

            string? username = principal?.Identity?.Name;

            Employee? user = _dbContext.Employees
                            .FirstOrDefault(x => x.Username == username);

            if (user is null)
                return Unauthorized("Invalid attempt");

            UserCredentials credentials = new()
            {
                EmailAddress = user.Email,
                Password = user.Password,
                UserName = user.Username
            };

            UserRefreshTokens? savedRefreshedToken = await _userRefreshTokenService
                                                    .GetSavedRefreshTokens(credentials, token.RefreshToken);

            if (savedRefreshedToken is null || savedRefreshedToken.RefreshToken != token.RefreshToken)
            {
                return Unauthorized("Invalid attempt");
            }

            var newToken = _jwtManagerRepository
                .GenerateRefreshToken(credentials);
            if (newToken == null)
            {
                return Unauthorized("Invalid attempt");
            }
            int.TryParse(_configuration["Jwt:RefreshTokenLifetime"], out int lifetime);

            UserRefreshTokens obj = new UserRefreshTokens
            {
                Employee = user,
                RefreshToken = newToken.RefreshToken,
                ExpirationDate = DateTimeOffset.UtcNow.AddMinutes(lifetime),
            };

            await _userRefreshTokenService.DeleteUserRefreshTokens(credentials, token.RefreshToken);
            await _userRefreshTokenService.AddUserRefreshTokens(obj);


            return Ok(newToken);



        }
    }
}
