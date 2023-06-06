using Application.Interfaces.ModelInterface;

namespace Infrastructure.Services
{
    public class CarRepository : Repository<Car>, ICarRepository
    {
        public CarRepository(IDealershipDbContext context) : base(context)
        {
        }
    }
}
