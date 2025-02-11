using Ats_Demo.Dtos;
using Ats_Demo.Services;
using MediatR;

namespace Ats_Demo.Features.Employee.Queries.GetAll
{
    public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, IEnumerable<EmployeeDetailsDto>>
    {
        private readonly IEmployeeService _employeeService;
        private readonly RedisCacheService _cacheService;


        public GetAllEmployeesQueryHandler(IEmployeeService employeeService, RedisCacheService cacheService)
        {
            _employeeService = employeeService;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<EmployeeDetailsDto>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            const string cacheKey = "AllEmployees";
            var cachedEmployees = await _cacheService.GetCachedDataAsync<IEnumerable<EmployeeDetailsDto>>(cacheKey);

            if (cachedEmployees is not null)
            {
                return cachedEmployees; 
            }

            var employees = await _employeeService.GetAllEmployees();
            await _cacheService.SetCacheDataAsync(cacheKey, employees, TimeSpan.FromMinutes(5));

            return employees;
        }
    }
}
