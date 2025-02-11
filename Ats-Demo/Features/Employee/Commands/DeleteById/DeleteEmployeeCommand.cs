using MediatR;

namespace Ats_Demo.Features.Employee.Commands.DeleteById
{
    public class DeleteEmployeeCommand : IRequest<string>
    {
        public Guid Id { get; set; }
    }
}
