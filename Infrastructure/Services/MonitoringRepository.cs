using Application.Interfaces.ModelInterface;

namespace Infrastructure.Services
{
    public class MonitoringRepository : Repository<Monitoring>, IMonitoringRepository
    {
        public MonitoringRepository(IDealershipDbContext context) : base(context)
        {
        }
    }
}
