using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces.ModelInterface;
using Application.Interfaces.ModelInterface.Login;
using Domain.Entities.IdentityEntities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Notification.Application.Interfaces;
using Notification.Domain.Models;

namespace Infrastructure.Services.JWTService
{
    public class TokenService : ITokenService
    {
        private readonly IDealershipDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPermissionRepository _permissionRepository;

        public TokenService(IDealershipDbContext dbContext, IConfiguration configuration, IEmployeeRepository employeeRepository, IPermissionRepository permissionRepository)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _employeeRepository = employeeRepository;
            _permissionRepository = permissionRepository;
        }

        public async Task<string?> CreateTokenAsync(UserCredentials credentials)
        {
            Employee? user = _dbContext.Employees.Where(o => o.FullName == credentials.UserName
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

            SecurityToken token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(5),
                claims: claims,
                notBefore: DateTime.Now,
                signingCredentials: signingCredentials
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;

        }
        private IEnumerable<Permission> GetEmployeePermissions(Guid employeeId)
        {
            IQueryable<Employee> employees = _employeeRepository.GetAsync(x => true, nameof(Employee.Roles)).Result;
            Employee? employee = employees.FirstOrDefault(emp => emp.Id == employeeId);
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

