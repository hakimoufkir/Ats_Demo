using Ats_Demo.Dtos;
using Ats_Demo.Exceptions;
using Ats_Demo.Services;
using MediatR;

namespace Ats_Demo.Features.Employee.Commands.Update
{
    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, EmployeeDetailsDto>
    {
        private readonly IEmployeeService _employeeService;

        public UpdateEmployeeCommandHandler(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public async Task<EmployeeDetailsDto> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            return await _employeeService.UpdateEmployee(request.Id, request.UpdateEmployeeDto);
        }
    }
}
