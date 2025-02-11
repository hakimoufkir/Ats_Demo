using Ats_Demo.Dtos;
using Ats_Demo.Exceptions;
using Ats_Demo.Services;
using AutoMapper;
using MediatR;

namespace Ats_Demo.Features.Employee.Commands.Add
{
    public class AddEmployeeCommandHandler : IRequestHandler<AddEmployeeCommand, Ats_Demo.Dtos.EmployeeDto>
    {
        private readonly IEmployeeService employeeService;
        private readonly IMapper mapper;

        public AddEmployeeCommandHandler(IEmployeeService employeeService, IMapper mapper)
        {
            this.employeeService = employeeService;
            this.mapper = mapper;
        }

        public async Task<Ats_Demo.Dtos.EmployeeDto> Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
        {
            if(request.employeeDto is null)
            {
                throw new EmployeeCreationException();
            }
            // Mapping Dto to Entity to send it to service 
            Ats_Demo.Entities.Employee employee = mapper.Map<Ats_Demo.Entities.Employee>(request.employeeDto);
            // Calling service to add employee
            Ats_Demo.Dtos.EmployeeDto result = await employeeService.AddEmployee(employee);
            return result;

        }
    }
}
