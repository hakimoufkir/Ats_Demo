using Ats_Demo.Dtos;
using MediatR;

namespace Ats_Demo.Features.Employee.Commands.Add
{
    public class AddEmployeeCommand : IRequest<EmployeeDetailsDto>
    {
        public CreateEmployeeDto? EmployeeDto { get; set; }

    }
}
