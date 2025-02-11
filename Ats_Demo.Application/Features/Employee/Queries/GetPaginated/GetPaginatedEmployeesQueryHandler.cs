using Ats_Demo.Application.Services;
using Ats_Demo.Domain.Dtos;
using AutoMapper;
using MediatR;


namespace Ats_Demo.Application.Features.Employee.Queries.GetPaginated;

public class GetPaginatedEmployeesQueryHandler : IRequestHandler<GetPaginatedEmployeesQuery, IEnumerable<EmployeeDetailsDto>>
{
    private readonly IUnitOfWork.IUnitOfWork _unitOfWork;
    private readonly IRedisCacheService _cacheService;
    private readonly IMapper _mapper;

    public GetPaginatedEmployeesQueryHandler(IRedisCacheService cacheService, IUnitOfWork.IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EmployeeDetailsDto>> Handle(GetPaginatedEmployeesQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"Employees_{request.Start}_{request.Limit}";
        var cachedEmployees = await _cacheService.GetCachedDataAsync<IEnumerable<EmployeeDetailsDto>>(cacheKey);

        if (cachedEmployees is not null)
        {
            return cachedEmployees;
        }

        var employees = await _unitOfWork.EmployeeReadRepository
            .GetAllAsync(skip: request.Start, take: request.Limit);

        if (employees == null || !employees.Any())
            throw new KeyNotFoundException("No employees found.");

        var employeeDtos = _mapper.Map<IEnumerable<EmployeeDetailsDto>>(employees);

        await _cacheService.SetCacheDataAsync(cacheKey, employeeDtos, TimeSpan.FromMinutes(5));

        return employeeDtos;
    }
}