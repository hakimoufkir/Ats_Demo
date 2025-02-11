using Ats_Demo.Data;
using Ats_Demo.GenericRepo;

namespace Ats_Demo.Repositories.EmployeeRepo
{
    public class EmployeeRepository : GenericRepository<Ats_Demo.Entities.Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
