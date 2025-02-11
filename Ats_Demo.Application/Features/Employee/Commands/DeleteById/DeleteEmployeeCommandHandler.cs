using Ats_Demo.Application.Services;
using Ats_Demo.Application.Exceptions;
using Ats_Demo.Application.IRepositories;
using MediatR;

namespace Ats_Demo.Application.Features.Employee.Commands.DeleteById
{
    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, string>
    {
        private readonly IUnitOfWork.IUnitOfWork _unitOfWork;
        private readonly IRedisCacheService _cacheService;


        public DeleteEmployeeCommandHandler(IUnitOfWork.IUnitOfWork unitOfWork, IRedisCacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task<string> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await _unitOfWork.EmployeeWriteRepository.GetAsync(e => e.Id == request.Id)
                ?? throw new EmployeeNotFoundException(request.Id);

            await _unitOfWork.EmployeeWriteRepository.RemoveAsync(employee);

            // Clear cache
            await _cacheService.RemoveCacheDataAsync("AllEmployees");
            await _cacheService.RemoveCacheDataAsync($"Employee_{request.Id}");

            return $"Employee with ID {request.Id} deleted successfully.";
        }
    }
}
