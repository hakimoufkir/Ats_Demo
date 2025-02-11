using Ats_Demo.Dtos;
using MediatR;

namespace Ats_Demo.Features.Employee.Commands.Update
{
    public class UpdateEmployeeCommand : IRequest<EmployeeDto>
    {
        public Guid Id { get; set; }
        public EmployeeDto UpdatedEmployeeDto { get; set; }
    }
}
