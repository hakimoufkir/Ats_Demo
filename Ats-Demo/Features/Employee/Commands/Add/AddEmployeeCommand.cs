using MediatR;

namespace Ats_Demo.Features.Employee.Commands.Add
{
    public class AddEmployeeCommand : IRequest<Ats_Demo.Dtos.EmployeeDto>
    {
        public Ats_Demo.Dtos.EmployeeDto? employeeDto { get; set; }
    }
}
