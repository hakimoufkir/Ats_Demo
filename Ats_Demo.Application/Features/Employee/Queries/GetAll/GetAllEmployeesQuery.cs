using Ats_Demo.Domain.Dtos;
using MediatR;

namespace Ats_Demo.Application.Features.Employee.Queries.GetAll
{
    public class GetAllEmployeesQuery : IRequest<IEnumerable<EmployeeDetailsDto>>
    {

    }
}
