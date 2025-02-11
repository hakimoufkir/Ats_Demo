using Ats_Demo.Application.Features.Employee.Commands.Add;
using Ats_Demo.Application.Features.Employee.Commands.DeleteById;
using Ats_Demo.Application.Features.Employee.Commands.Update;
using Ats_Demo.Application.Features.Employee.Queries.GetAll;
using Ats_Demo.Application.Features.Employee.Queries.GetById;
using Ats_Demo.Application.Features.Employee.Queries.GetPaginated;
using Ats_Demo.Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ats_Demo.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeeController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _mediator.Send(new GetAllEmployeesQuery());
            return Ok(employees);
        }

        [HttpGet("paginated")]
        public async Task<IActionResult> GetPaginatedEmployees([FromQuery] int start = 0, [FromQuery] int limit = 10)
        {
            var query = new GetPaginatedEmployeesQuery(start, limit);
            var employees = await _mediator.Send(query);
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(Guid id)
        {
            var employee = await _mediator.Send(new GetEmployeeByIdQuery(id));
            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] AddEmployeeCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdEmployee = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetEmployeeById), new { id = createdEmployee.Id }, createdEmployee);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(Guid id, [FromBody] UpdateEmployeeDto updateEmployeeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new UpdateEmployeeCommand(id, updateEmployeeDto);
            var updatedEmployee = await _mediator.Send(command);
    
            return Ok(updatedEmployee);
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            var result = await _mediator.Send(new DeleteEmployeeCommand { Id = id });
            return Ok(new { Message = result });
        }
    }
}
