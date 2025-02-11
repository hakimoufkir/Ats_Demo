using MediatR;

namespace Ats_Demo.Application.Features.Employee.Commands.DeleteById
{
    public class DeleteEmployeeCommand : IRequest<string>
    {
        public Guid Id { get; set; }
    }
}
