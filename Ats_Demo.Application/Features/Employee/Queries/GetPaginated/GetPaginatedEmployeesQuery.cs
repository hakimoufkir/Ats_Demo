using Ats_Demo.Domain.Dtos;
using MediatR;

namespace Ats_Demo.Application.Features.Employee.Queries.GetPaginated;

public class GetPaginatedEmployeesQuery: IRequest<IEnumerable<EmployeeDetailsDto>>
{
    public int Start { get; set; }
    public int Limit { get; set; }

    public GetPaginatedEmployeesQuery(int start, int limit)
    {
        Start = start;
        Limit = limit;
    }
}