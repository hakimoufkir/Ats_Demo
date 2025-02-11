using Ats_Demo.Entities;

namespace Ats_Demo.Repositories.EmployeeRepo
{
    public interface IEmployeeReadRepository
    {
        Task DeleteEmployeeAsync(Guid id);
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(Guid id);
        Task InsertEmployeeAsync(Employee employee);
        Task UpdateEmployeeAsync(Employee employee);
    }
}