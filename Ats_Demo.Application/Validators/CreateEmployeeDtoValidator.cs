using Ats_Demo.Domain.Dtos;
using FluentValidation;

namespace Ats_Demo.Application.Validators
{
    public class CreateEmployeeDtoValidator : AbstractValidator<CreateEmployeeDto>
    {
        public CreateEmployeeDtoValidator()
        {
            RuleFor(e => e.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");

            RuleFor(e => e.Position)
                .NotEmpty().WithMessage("Position is required.")
                .MaximumLength(100).WithMessage("Position cannot exceed 100 characters.");

            RuleFor(e => e.Office)
                .NotEmpty().WithMessage("Office is required.")
                .MaximumLength(100).WithMessage("Office cannot exceed 100 characters.");

            RuleFor(e => e.Age)
                .GreaterThan(18).WithMessage("Age must be greater than 18.")
                .LessThan(100).WithMessage("Age must be less than 100.");

            RuleFor(e => e.Salary)
                .GreaterThan(0).WithMessage("Salary must be a positive number.");
        }
    }
}
