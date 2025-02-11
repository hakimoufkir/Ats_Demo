using Ats_Demo.Dtos;
using Ats_Demo.Entities;

namespace Ats_Demo.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Ats_Demo.Entities.Employee>> GetAllEmployees();
        Task<Ats_Demo.Entities.Employee> GetEmployeeById(Guid id);
        Task<Ats_Demo.Dtos.EmployeeDto> AddEmployee(Employee employee);
        Task<EmployeeDto> UpdateEmployee(Guid id, EmployeeDto updatedEmployeeDto);
        Task<string> DeleteEmployeeById(Guid id);
    }
}
