using Ats_Demo.Application.IRepositories;
using Ats_Demo.Application.Services;
using Ats_Demo.Domain.Dtos;
using AutoMapper;
using MediatR;

namespace Ats_Demo.Application.Features.Employee.Queries.GetAll
{
    public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, IEnumerable<EmployeeDetailsDto>>
    {
        private readonly IUnitOfWork.IUnitOfWork _unitOfWork;
        private readonly IRedisCacheService _cacheService;
        private readonly IMapper _mapper;

        public GetAllEmployeesQueryHandler(IRedisCacheService cacheService,IUnitOfWork.IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeDetailsDto>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            const string cacheKey = "AllEmployees";
            var cachedEmployees = await _cacheService.GetCachedDataAsync<IEnumerable<EmployeeDetailsDto>>(cacheKey);

            if (cachedEmployees is not null)
            {
                return cachedEmployees;
            }

            IEnumerable<Domain.Entities.Employee> employees = await _unitOfWork.EmployeeReadRepository.GetAllAsync();
            if (employees == null)
                throw new KeyNotFoundException("No employees found.");

            await _cacheService.SetCacheDataAsync(cacheKey, employees, TimeSpan.FromMinutes(5));
            return _mapper.Map<IEnumerable<EmployeeDetailsDto>>(employees);
        }
    }
}
