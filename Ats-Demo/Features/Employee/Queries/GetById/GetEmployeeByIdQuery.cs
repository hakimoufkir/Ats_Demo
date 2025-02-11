using Ats_Demo.Dtos;
using MediatR;

namespace Ats_Demo.Features.Employee.Queries.GetById
{
    public class GetEmployeeByIdQuery : IRequest<EmployeeDetailsDto>
    {
        public Guid Id { get; set; }
 
    }
}
