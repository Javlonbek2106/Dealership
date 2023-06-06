using Application.Interfaces.ModelInterface.Login;
using Domain.Entities.IdentityEntities;

namespace Infrastructure.Services.Login
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(IDealershipDbContext context) : base(context)
        {
        }
    }
}
