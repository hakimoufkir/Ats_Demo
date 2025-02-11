using Ats_Demo.Dtos;
using MediatR;

namespace Ats_Demo.Features.Employee.Commands.Update
{
    public class UpdateEmployeeCommand : IRequest<EmployeeDetailsDto>
    {
        public Guid Id { get; set; }
        public UpdateEmployeeDto UpdateEmployeeDto { get; set; }
    }
}
