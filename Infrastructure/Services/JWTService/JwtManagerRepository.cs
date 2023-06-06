using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.ModelInterface;
using Application.Interfaces.ModelInterface.Login;
using Domain.Entities.IdentityEntities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Notification.Application.Interfaces;
using Notification.Domain.Models;

namespace Infrastructure.Services.JWTService
{
    public class JwtManagerRepository : IJwtManagerRepository
    {
        private readonly IDealershipDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPermissionRepository _permissionRepository;

        public JwtManagerRepository(IDealershipDbContext dbContext, IConfiguration configuration, IEmployeeRepository employeeRepository, IPermissionRepository permissionRepository)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _employeeRepository = employeeRepository;
            _permissionRepository = permissionRepository;
        }

        public Token? GenerateRefreshToken(UserCredentials credentials)
        {
            Employee? user = _dbContext.Employees.Where(o => o.Username == credentials.UserName
                                        && o.Email == credentials.EmailAddress
                                        && o.Password == credentials.Password)?                                       
                                        .FirstOrDefault();

            if (user == null) return null;

            IEnumerable<Role?> userRoles = user.Roles;

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName, user.FullName)
            };
            List<string?> permissions = new List<string?>();

            foreach (Permission permission in GetEmployeePermissions(user.Id))
            {
                permissions.Add(permission.PermissionName);
            }

            permissions = permissions.Distinct().ToList();

            foreach (string? permission in permissions)
            {
                claims.Add(new Claim(ClaimTypes.Role, permission));
            }


            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            int.TryParse(_configuration["Jwt:RefreshTokenLifetime"], out int lifetime);

            SecurityToken token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(lifetime),
                claims: claims,
                signingCredentials: signingCredentials
                );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            return new Token { AccessToken = accessToken, RefreshToken = refreshToken };
        }


        public Token? GenerateAccessTokens(UserCredentials credentials)
        {

            Employee? user = _dbContext.Employees.Where(o => o.Username == credentials.UserName
                                        && o.Email == credentials.EmailAddress
                                        && o.Password == credentials.Password)?
                                        .FirstOrDefault();

            if (user == null) return null;

            IEnumerable<Role?> userRoles = user.Roles;

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName, user.FullName)
            };
            List<string?> permissions = new List<string?>();

            foreach (Permission permission in GetEmployeePermissions(user.Id))
            {
                permissions.Add(permission.PermissionName);
            }

            permissions = permissions.Distinct().ToList();

            foreach (string? permission in permissions)
            {
                claims.Add(new Claim(ClaimTypes.Role, permission));
            }



            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            int.TryParse(_configuration["Jwt:AccessTokenLifetime"], out int lifetime);

            SecurityToken token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(lifetime),
                claims: claims,
                signingCredentials: signingCredentials
                );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            return new Token { AccessToken = accessToken, RefreshToken = refreshToken };
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {

            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidIssuer = _configuration["Jwt:Issuer"],
                RequireExpirationTime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principal;

        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new Byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }



        private IEnumerable<Permission> GetEmployeePermissions(Guid employeeId)
        {
            IQueryable<Employee> employees = _employeeRepository.GetAsync(x=>true, nameof(Employee.Roles)).Result;
            Employee? employee = employees.FirstOrDefault(emp=>emp.Id == employeeId);
            IEnumerable<Role> roles = employee.Roles;

            List<Permission> permissions = new List<Permission>();

            foreach (Role role in roles)
            {
                IEnumerable<Permission> rolePermissions = _permissionRepository.GetAsync(p => p.Roles.Contains(role)).Result;
                permissions.AddRange(rolePermissions);
            }

            return permissions;
        }

    }
}
