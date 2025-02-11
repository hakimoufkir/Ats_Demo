using Ats_Demo.Domain.Entities;

namespace Ats_Demo.Application.IRepositories
{
    public interface IEmployeeReadRepository
    {
        Task DeleteEmployeeAsync(Guid id);
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(Guid id);
        Task InsertEmployeeAsync(Employee employee);
        Task UpdateEmployeeAsync(Employee employee);
        Task<IEnumerable<Employee>> GetAllAsync(int skip = 0, int take = 10);

    }
}