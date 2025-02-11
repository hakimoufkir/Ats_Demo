using Ats_Demo.Dtos;
using Ats_Demo.Exceptions;
using Ats_Demo.Services;
using AutoMapper;
using MediatR;

namespace Ats_Demo.Features.Employee.Commands.Add
{
    public class AddEmployeeCommandHandler : IRequestHandler<AddEmployeeCommand, EmployeeDetailsDto>
    {
        private readonly IEmployeeService employeeService;
        private readonly IMapper mapper;

        public AddEmployeeCommandHandler(IEmployeeService employeeService, IMapper mapper)
        {
            this.employeeService = employeeService;
            this.mapper = mapper;
        }

        public async Task<Ats_Demo.Dtos.EmployeeDetailsDto> Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
        {
            EmployeeDetailsDto result = await employeeService.AddEmployee(request.EmployeeDto);
            return result;

        }
    }
}
