using Ats_Demo.Features.Employee.Commands.Add;
using Ats_Demo.Features.Employee.Commands.DeleteById;
using Ats_Demo.Features.Employee.Commands.Update;
using Ats_Demo.Features.Employee.Queries.GetAll;
using Ats_Demo.Features.Employee.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ats_Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                var result = await _mediator.Send(new GetAllEmployeesQuery());
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> GetEmployeeById(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new GetEmployeeByIdQuery { Id = id});
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddEmployee([FromBody] AddEmployeeCommand command)
        {
            if (command == null)
            {
                return BadRequest("Invalid request");
            }
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(Guid id, [FromBody] UpdateEmployeeCommand command)
        {
            if (command == null)
            {
                return BadRequest("Invalid request");
            }
            try
            {
                command.Id = id;
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            try
            {
                string result = await _mediator.Send(new DeleteEmployeeCommand { Id = id });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
