using Ats_Demo.Application.Exceptions;
using Ats_Demo.Application.IRepositories;
using AutoMapper;
using MediatR;
using Ats_Demo.Domain.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;
using Ats_Demo.Application.Services;

namespace Ats_Demo.Application.Features.Employee.Commands.Update
{
    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, EmployeeDetailsDto>
    {
        private readonly IUnitOfWork.IUnitOfWork _unitOfWork;
        private readonly IRedisCacheService _cacheService;
        private readonly IMapper _mapper;

        public UpdateEmployeeCommandHandler(IUnitOfWork.IUnitOfWork unitOfWork, IRedisCacheService cacheService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _mapper = mapper;
        }

        public async Task<EmployeeDetailsDto> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            if (request.UpdateEmployeeDto == null)
                throw new EmployeeUpdateException("Update request cannot be null.");

            var existingEmployee = await _unitOfWork.EmployeeReadRepository.GetByIdAsync(request.Id);
            
            if (existingEmployee == null)
                throw new EmployeeNotFoundException(request.Id);

            // Ensure that at least one field is being updated
            if (string.IsNullOrEmpty(request.UpdateEmployeeDto.Name) &&
                string.IsNullOrEmpty(request.UpdateEmployeeDto.Position) &&
                string.IsNullOrEmpty(request.UpdateEmployeeDto.Office) &&
                !request.UpdateEmployeeDto.Age.HasValue &&
                !request.UpdateEmployeeDto.Salary.HasValue)
            {
                throw new EmployeeUpdateException("No valid fields provided for update.");
            }

            // Update only modified attributes
            existingEmployee.Name = request.UpdateEmployeeDto.Name ?? existingEmployee.Name;
            existingEmployee.Position = request.UpdateEmployeeDto.Position ?? existingEmployee.Position;
            existingEmployee.Office = request.UpdateEmployeeDto.Office ?? existingEmployee.Office;
            existingEmployee.Age = request.UpdateEmployeeDto.Age ?? existingEmployee.Age;
            existingEmployee.Salary = request.UpdateEmployeeDto.Salary ?? existingEmployee.Salary;
            existingEmployee.LastModifiedDate = DateTime.UtcNow;

            await _unitOfWork.EmployeeWriteRepository.UpdateAsync(existingEmployee);

            // Clear cache
            await _cacheService.RemoveCacheDataAsync("AllEmployees");
            await _cacheService.RemoveCacheDataAsync($"Employee_{request.Id}");

            return _mapper.Map<EmployeeDetailsDto>(existingEmployee);
        }
    }
}
