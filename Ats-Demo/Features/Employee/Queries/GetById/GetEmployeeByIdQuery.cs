using MediatR;

namespace Ats_Demo.Features.Employee.Queries.GetById
{
    public class GetEmployeeByIdQuery : IRequest<Ats_Demo.Entities.Employee>
    {
        public Guid Id { get; set; }
 
    }
}
