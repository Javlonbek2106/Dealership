using Domain.Entities.IdentityEntities;
using Microsoft.EntityFrameworkCore;
using Notification.Domain.Models;

namespace Application.Abstraction
{
    public interface IDealershipDbContext
    {
        DbSet<T> Set<T>() where T : class;
        public DbSet<Car> Cars { get; }
        public DbSet<Dealership> Dealerships { get; }
        public DbSet<Employee> Employees { get; }
        public DbSet<SoldCar> SoldCars { get; }
        public DbSet<Monitoring> Monitorings { get; }
        public DbSet<Permission> Permissions { get; }
        public DbSet<Role> Roles { get; }
        public DbSet<UserRefreshTokens> UserRefreshToken { get; }


        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
