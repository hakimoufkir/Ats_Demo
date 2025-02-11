using Ats_Demo.Services;
using MediatR;

namespace Ats_Demo.Features.Employee.Commands.DeleteById
{
    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, string>
    {
        private readonly IEmployeeService _employeeService;

        public DeleteEmployeeCommandHandler(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public async Task<string> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            string res = await _employeeService.DeleteEmployeeById(request.Id);
            return res;
        }
    }
}
