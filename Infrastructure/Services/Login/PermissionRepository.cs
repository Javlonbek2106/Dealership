using Application.Abstraction;
using Application.Interfaces.ModelInterface.Login;
using Domain.Entities.IdentityEntities;

namespace Infrastructure.Services.Login
{
    public class PermissionRepository : Repository<Permission>, IPermissionRepository
    {
        public PermissionRepository(IDealershipDbContext context) : base(context)
        {
        }
    }
}
