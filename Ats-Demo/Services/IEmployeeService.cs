using Ats_Demo.Dtos;

namespace Ats_Demo.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDetailsDto>> GetAllEmployees();
        Task<EmployeeDetailsDto> GetEmployeeById(Guid id);
        Task<EmployeeDetailsDto> AddEmployee(CreateEmployeeDto employeeDto);
        Task<EmployeeDetailsDto> UpdateEmployee(Guid id, UpdateEmployeeDto updatedEmployeeDto);
        Task<string> DeleteEmployeeById(Guid id);
    }
}
