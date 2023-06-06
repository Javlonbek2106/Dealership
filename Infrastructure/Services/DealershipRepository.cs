using Application.Interfaces.ModelInterface;

namespace Infrastructure.Services
{
    public class DealershipRepository : Repository<Dealership>, IDealershipRepository
    {
        public DealershipRepository(IDealershipDbContext context) : base(context)
        {
        }
    }
}
