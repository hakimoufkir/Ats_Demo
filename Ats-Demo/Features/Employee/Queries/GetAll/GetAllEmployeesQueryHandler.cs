using Ats_Demo.Services;
using MediatR;

namespace Ats_Demo.Features.Employee.Queries.GetAll
{
    public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, IEnumerable<Entities.Employee>>
    {
        private readonly IEmployeeService _employeeService;

        public GetAllEmployeesQueryHandler(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public async Task<IEnumerable<Entities.Employee>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Entities.Employee> employees = await _employeeService.GetAllEmployees();
            return employees;
        }
    }
}
