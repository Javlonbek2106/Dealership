using Domain.Entities.IdentityEntities;
using Infrastructure.DataAccess.Interceptor;
using Microsoft.EntityFrameworkCore;
using Notification.Domain.Models;

namespace Infrastructure.DataAccess
{
    public class DealershipDbContext : DbContext, IDealershipDbContext
    {
        private readonly AuditableEntitySaveChangesInterceptor _interceptor;
        public DealershipDbContext(DbContextOptions<DealershipDbContext> options,
            AuditableEntitySaveChangesInterceptor interceptor)
            : base(options)
        {
            _interceptor = interceptor;
        }

        public DbSet<Car> Cars { get; set; }

        public DbSet<Dealership> Dealerships{ get; set; }

        public DbSet<Employee> Employees{ get; set; }

        public DbSet<SoldCar> SoldCars{ get; set; }

        public DbSet<Monitoring> Monitorings{ get; set; }

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRefreshTokens> UserRefreshToken { get; set; }
    }
}
