using Ats_Demo.Exceptions;
using Ats_Demo.Services;
using MediatR;

namespace Ats_Demo.Features.Employee.Queries.GetById
{
    public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, Ats_Demo.Entities.Employee>
    {
        private readonly IEmployeeService employeeService;

        public GetEmployeeByIdQueryHandler(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }
        public async Task<Entities.Employee> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            Ats_Demo.Entities.Employee employee = await employeeService.GetEmployeeById(request.Id);
            if (employee == null)
            {
                throw new EmployeeNotFoundException(request.Id);
            }
            return employee;
        }
    }
}
