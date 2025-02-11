using Ats_Demo.Application.Exceptions;
using Ats_Demo.Application.IRepositories;
using Ats_Demo.Application.Services;
using Ats_Demo.Domain.Dtos;
using AutoMapper;
using MediatR;

namespace Ats_Demo.Application.Features.Employee.Queries.GetById
{
    public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDetailsDto>
    {
        private readonly IUnitOfWork.IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRedisCacheService _cacheService;

        public GetEmployeeByIdQueryHandler(IUnitOfWork.IUnitOfWork unitOfWork, IMapper mapper, IRedisCacheService cacheService)
        {
            _unitOfWork = unitOfWork;
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

            var employee = await _unitOfWork.EmployeeReadRepository.GetByIdAsync(request.Id)
                ?? throw new EmployeeNotFoundException(request.Id);


            await _cacheService.SetCacheDataAsync(cacheKey, employee, TimeSpan.FromMinutes(5));

            return _mapper.Map<EmployeeDetailsDto>(employee);
        }
    }
}
