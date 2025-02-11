using Ats_Demo.Domain.Dtos;
using MediatR;
using System.Text.Json.Serialization;

namespace Ats_Demo.Application.Features.Employee.Commands.Update
{
    public class UpdateEmployeeCommand : IRequest<EmployeeDetailsDto>
    {
        [JsonIgnore] // This prevents the Id from being part of the request body
        public Guid Id { get; private set; }
        public UpdateEmployeeDto UpdateEmployeeDto { get; set; }

        public UpdateEmployeeCommand(Guid id, UpdateEmployeeDto updateEmployeeDto)
        {
            Id = id;
            UpdateEmployeeDto = updateEmployeeDto;
        }
    }
}