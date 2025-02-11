using FluentValidation;
using Ats_Demo.Application.Features.Employee.Queries.GetById;

namespace Ats_Demo.Application.Validators
{
    public class GetEmployeeByIdQueryValidator : AbstractValidator<GetEmployeeByIdQuery>
    {
        public GetEmployeeByIdQueryValidator()
        {
            RuleFor(q => q.Id)
                .NotEmpty().WithMessage("Employee ID is required.")
                .Must(id => Guid.TryParse(id.ToString(), out _)).WithMessage("Invalid Employee ID format.");
        }
    }
}
