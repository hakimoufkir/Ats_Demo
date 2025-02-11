using Ats_Demo.Domain.Dtos;

using MediatR;

namespace Ats_Demo.Application.Features.Employee.Queries.GetById
{
    public class GetEmployeeByIdQuery : IRequest<EmployeeDetailsDto>
    {
        public Guid Id { get; set; }

        public GetEmployeeByIdQuery(Guid id)
        {
            Id = id;
        }

    }
}
