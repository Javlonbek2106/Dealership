using Application.Interfaces.ModelInterface;

namespace Infrastructure.Services
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(IDealershipDbContext context) : base(context)
        {
        }
    }
}
