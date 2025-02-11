using Ats_Demo.Dtos;
using Ats_Demo.Entities;
using Ats_Demo.Exceptions;
using Ats_Demo.Repositories.EmployeeRepo;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ats_Demo.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeWriteRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly RedisCacheService _cacheService;

        public EmployeeService(IEmployeeWriteRepository employeeRepository, IMapper mapper, RedisCacheService cacheService)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<EmployeeDetailsDto>> GetAllEmployees()
        {
            var employees = await _employeeRepository.GetAllAsync();
            if (employees == null || employees.Count == 0)
                throw new KeyNotFoundException("No employees found.");

            return _mapper.Map<IEnumerable<EmployeeDetailsDto>>(employees);
        }

        public async Task<EmployeeDetailsDto> GetEmployeeById(Guid id)
        {
            var employee = await _employeeRepository.GetAsync(e => e.Id == id)
                ?? throw new EmployeeNotFoundException(id);

            return _mapper.Map<EmployeeDetailsDto>(employee);
        }

        public async Task<EmployeeDetailsDto> AddEmployee(CreateEmployeeDto employeeDto)
        {
            if (employeeDto == null)
                throw new ArgumentNullException(nameof(employeeDto), "Employee data cannot be null.");

            var employee = _mapper.Map<Employee>(employeeDto);
            employee.CreatedDate = DateTime.UtcNow;

            // Generate new ID only if not provided
            employee.Id = employee.Id == Guid.Empty ? Guid.NewGuid() : employee.Id;

            await _employeeRepository.CreateAsync(employee);

            // Clear cache
            await _cacheService.RemoveCacheDataAsync("AllEmployees");

            return _mapper.Map<EmployeeDetailsDto>(employee);
        }

        public async Task<EmployeeDetailsDto> UpdateEmployee(Guid id, UpdateEmployeeDto updatedEmployeeDto)
        {
            if (updatedEmployeeDto == null)
                throw new EmployeeUpdateException("Update request cannot be null.");

            var existingEmployee = await _employeeRepository.GetAsync(e => e.Id == id);
            if (existingEmployee == null)
                throw new EmployeeNotFoundException(id);

            // Ensure that at least one field is being updated
            if (string.IsNullOrEmpty(updatedEmployeeDto.Name) &&
                string.IsNullOrEmpty(updatedEmployeeDto.Position) &&
                string.IsNullOrEmpty(updatedEmployeeDto.Office) &&
                !updatedEmployeeDto.Age.HasValue &&
                !updatedEmployeeDto.Salary.HasValue)
            {
                throw new EmployeeUpdateException("No valid fields provided for update.");
            }

            // Update only modified attributes
            existingEmployee.Name = updatedEmployeeDto.Name ?? existingEmployee.Name;
            existingEmployee.Position = updatedEmployeeDto.Position ?? existingEmployee.Position;
            existingEmployee.Office = updatedEmployeeDto.Office ?? existingEmployee.Office;
            existingEmployee.Age = updatedEmployeeDto.Age ?? existingEmployee.Age;
            existingEmployee.Salary = updatedEmployeeDto.Salary ?? existingEmployee.Salary;
            existingEmployee.LastModifiedDate = DateTime.UtcNow;

            await _employeeRepository.UpdateAsync(existingEmployee);


            // Clear cache
            await _cacheService.RemoveCacheDataAsync("AllEmployees");
            await _cacheService.RemoveCacheDataAsync($"Employee_{id}");

            return _mapper.Map<EmployeeDetailsDto>(existingEmployee);
        }

        public async Task<string> DeleteEmployeeById(Guid id)
        {
            var employee = await _employeeRepository.GetAsync(e => e.Id == id)
                ?? throw new EmployeeNotFoundException(id);

            await _employeeRepository.RemoveAsync(employee);

            // Clear cache
            await _cacheService.RemoveCacheDataAsync("AllEmployees");
            await _cacheService.RemoveCacheDataAsync($"Employee_{id}");

            return $"Employee with ID {id} deleted successfully.";
        }
    }
}
