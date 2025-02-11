using Ats_Demo.Domain.Dtos;
using FluentValidation;

namespace Ats_Demo.Application.Validators
{
    public class UpdateEmployeeDtoValidator : AbstractValidator<UpdateEmployeeDto>
    {
        public UpdateEmployeeDtoValidator()
        {
            RuleFor(e => e.Name)
                .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.")
                .When(e => !string.IsNullOrEmpty(e.Name));

            RuleFor(e => e.Position)
                .MaximumLength(100).WithMessage("Position cannot exceed 100 characters.")
                .When(e => !string.IsNullOrEmpty(e.Position));

            RuleFor(e => e.Office)
                .MaximumLength(100).WithMessage("Office cannot exceed 100 characters.")
                .When(e => !string.IsNullOrEmpty(e.Office));

            RuleFor(e => e.Age)
                .GreaterThan(18).WithMessage("Age must be greater than 18.")
                .LessThan(100).WithMessage("Age must be less than 100.")
                .When(e => e.Age.HasValue);

            RuleFor(e => e.Salary)
                .GreaterThan(0).WithMessage("Salary must be a positive number.")
                .When(e => e.Salary.HasValue);
        }
    }
}
