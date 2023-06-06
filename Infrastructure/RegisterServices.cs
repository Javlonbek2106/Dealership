using Application.Interfaces.ModelInterface;
using Application.Interfaces.ModelInterface.Login;
using Infrastructure.DataAccess;
using Infrastructure.DataAccess.Interceptor;
using Infrastructure.Services;
using Infrastructure.Services.JWTService;
using Infrastructure.Services.Login;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notification.Application.Interfaces;

namespace Infrastructure
{
    public static class RegisterServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<AuditableEntitySaveChangesInterceptor>();
            services.AddDbContext<IDealershipDbContext, DealershipDbContext>(x => x.UseNpgsql(configuration.GetConnectionString("DbConnection")));
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            services.AddScoped<ICarRepository, CarRepository>();
            services.AddScoped<IDealershipRepository, DealershipRepository>();
            services.AddScoped<IMonitoringRepository, MonitoringRepository>();
            services.AddScoped<ISoldCarRepository, SoldCarRepository>();
            services.AddScoped<IJwtManagerRepository, JwtManagerRepository>();
            services.AddScoped<IUserRefreshTokenService, UserRefreshTokenService>();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
