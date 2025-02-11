using Ats_Demo.Entities;
using MediatR;

namespace Ats_Demo.Features.Employee.Queries.GetAll
{
    public class GetAllEmployeesQuery : IRequest<IEnumerable<Entities.Employee>>
    {

    }
}
