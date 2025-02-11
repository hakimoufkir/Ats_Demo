using Ats_Demo.Dtos;
using Ats_Demo.Entities;
using Ats_Demo.Exceptions;
using Ats_Demo.Repositories.EmployeeRepo;
using AutoMapper;
using MediatR;

namespace Ats_Demo.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<Ats_Demo.Dtos.EmployeeDto> AddEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            try
            {
                employee.CreatedDate = DateTime.UtcNow;
                employee.Id = Guid.NewGuid();
                await _employeeRepository.CreateAsync(employee);

                return _mapper.Map<Ats_Demo.Dtos.EmployeeDto>(employee);
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding employee: " + ex.Message);
            }
        }

        public async Task<string> DeleteEmployeeById(Guid id)
        {
            try
            {
                Employee? employee = await _employeeRepository.GetAsync((u) => u.Id == id) ?? throw new EmployeeNotFoundException(id);
                await _employeeRepository.RemoveAsync(employee);
                return "Employee deleted successfully.";
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting employee: " + ex.Message);
            }
        }

        public async Task<IEnumerable<Ats_Demo.Entities.Employee>> GetAllEmployees()
        {
            List<Ats_Demo.Entities.Employee>? employees = await _employeeRepository.GetAllAsync();
            return employees ?? new List<Ats_Demo.Entities.Employee>();
        }

        public async Task<Employee?> GetEmployeeById(Guid id)
        {
            Ats_Demo.Entities.Employee? employee = await _employeeRepository.GetAsync((u) => u.Id == id);
            return employee;
        }

        public async Task<EmployeeDto> UpdateEmployee(Guid id, EmployeeDto updatedEmployeeDto)
        {
            var existingEmployee = await _employeeRepository.GetAsync((u)=>u.Id == id);

            if (existingEmployee == null)
            {
                throw new KeyNotFoundException("Employee not found.");
            }

            // Update only the modified attributes
            existingEmployee.Name = updatedEmployeeDto.Name ?? existingEmployee.Name;
            existingEmployee.Position = updatedEmployeeDto.Position ?? existingEmployee.Position;
            existingEmployee.Office = updatedEmployeeDto.Office ?? existingEmployee.Office;
            existingEmployee.Age = updatedEmployeeDto.Age ?? existingEmployee.Age;
            existingEmployee.Salary = updatedEmployeeDto.Salary ?? existingEmployee.Salary;
            existingEmployee.LastModifiedDate = DateTime.UtcNow;

            await _employeeRepository.UpdateAsync(existingEmployee);

            return new EmployeeDto
            {
                Name = existingEmployee.Name,
                Position = existingEmployee.Position,
                Office = existingEmployee.Office,
                Age = existingEmployee.Age,
                Salary = existingEmployee.Salary
            };
        }
    }
}
