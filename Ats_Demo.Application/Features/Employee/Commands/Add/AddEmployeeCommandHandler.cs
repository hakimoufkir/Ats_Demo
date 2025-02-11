using Ats_Demo.Application.Services;
using Ats_Demo.Domain.Dtos;
using AutoMapper;
using MediatR;
using Ats_Demo.Application.IRepositories;

namespace Ats_Demo.Application.Features.Employee.Commands.Add
{
    public class AddEmployeeCommandHandler : IRequestHandler<AddEmployeeCommand, EmployeeDetailsDto>
    {
        private readonly IUnitOfWork.IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRedisCacheService _cacheService;

        public AddEmployeeCommandHandler(IUnitOfWork.IUnitOfWork unitOfWork, IMapper mapper, IRedisCacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<EmployeeDetailsDto> Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
        {
            
            var EmployeeDto = new Domain.Entities.Employee
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Position = request.Position,
                Office = request.Office,
                Age = request.Age,
                Salary = request.Salary,
                CreatedDate = DateTime.UtcNow
            };


            if (EmployeeDto is null)
                throw new ArgumentNullException(nameof(EmployeeDto), "Employee data cannot be null.");

            var employee = _mapper.Map<Ats_Demo.Domain.Entities.Employee>(EmployeeDto);
            employee.CreatedDate = DateTime.UtcNow;

            // Generate new ID only if not provided
            employee.Id = employee.Id == Guid.Empty ? Guid.NewGuid() : employee.Id;

            await _unitOfWork.EmployeeWriteRepository.CreateAsync(employee);

            // Clear cache
            await _cacheService.RemoveCacheDataAsync("AllEmployees");

            return _mapper.Map<EmployeeDetailsDto>(employee);

        }
    }
}
