using System.ComponentModel.DataAnnotations.Schema;
using Ats_Demo.Domain.Dtos;
using MediatR;

namespace Ats_Demo.Application.Features.Employee.Commands.Add
{
    public class AddEmployeeCommand : IRequest<EmployeeDetailsDto>
    {
        public string? Name { get; set; }
        public string? Position { get; set; }
        public string? Office { get; set; }
        public int? Age { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Salary { get; set; }

        public AddEmployeeCommand(string? name, string? position, string? office, int? age, decimal? salary)
        {
            Name = name;
            Position = position;
            Office = office;
            Age = age;
            Salary = salary;
        }
    }
}
