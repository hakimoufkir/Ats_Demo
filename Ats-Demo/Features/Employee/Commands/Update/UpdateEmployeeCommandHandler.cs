using Ats_Demo.Dtos;
using Ats_Demo.Exceptions;
using Ats_Demo.Services;
using MediatR;

namespace Ats_Demo.Features.Employee.Commands.Update
{
    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, Ats_Demo.Dtos.EmployeeDto>
    {
        private readonly IEmployeeService _employeeService;

        public UpdateEmployeeCommandHandler(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public async Task<EmployeeDto> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new EmployeeUpdateException();
            }
            EmployeeDto updatedEmployeeDto = await _employeeService.UpdateEmployee(request.Id, request.UpdatedEmployeeDto);
            return updatedEmployeeDto;
        }
    }
}
