using Ats_Demo.Dtos;
using Ats_Demo.Entities;
using MediatR;

namespace Ats_Demo.Features.Employee.Queries.GetAll
{
    public class GetAllEmployeesQuery : IRequest<IEnumerable<EmployeeDetailsDto>>
    {

    }
}
