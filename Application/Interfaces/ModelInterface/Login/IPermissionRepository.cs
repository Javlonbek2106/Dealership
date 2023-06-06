using Application.Repository;
using Domain.Entities.IdentityEntities;

namespace Application.Interfaces.ModelInterface.Login
{
    public interface IPermissionRepository : IRepository<Permission>
    {
    }
}
