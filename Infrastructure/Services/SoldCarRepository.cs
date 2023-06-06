using Application.Interfaces.ModelInterface;

namespace Infrastructure.Services
{
    public class SoldCarRepository : Repository<SoldCar>, ISoldCarRepository
    {
        public SoldCarRepository(IDealershipDbContext context) : base(context)
        {
        }
    }
}
