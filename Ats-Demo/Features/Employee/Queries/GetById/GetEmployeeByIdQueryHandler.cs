using Ats_Demo.Dtos;
using Ats_Demo.Exceptions;
using Ats_Demo.Services;
using AutoMapper;
using MediatR;

namespace Ats_Demo.Features.Employee.Queries.GetById
{
    public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDetailsDto>
    {
        private readonly IEmployeeService employeeService;
        private readonly IMapper _mapper;
        private readonly RedisCacheService _cacheService;

        public GetEmployeeByIdQueryHandler(IEmployeeService employeeService, IMapper mapper, RedisCacheService cacheService)
        {
            this.employeeService = employeeService;
            _mapper = mapper;
            _cacheService = cacheService;
        }
        public async Task<EmployeeDetailsDto> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"Employee_{request.Id}";
            var cachedEmployee = await _cacheService.GetCachedDataAsync<EmployeeDetailsDto>(cacheKey);

            if (cachedEmployee is not null)
            {
                return cachedEmployee; // Return cached data
            }

            var employee = await employeeService.GetEmployeeById(request.Id);
            await _cacheService.SetCacheDataAsync(cacheKey, employee, TimeSpan.FromMinutes(5));

            return employee;
        }
    }
}
