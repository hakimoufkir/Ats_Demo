using FluentValidation;
using Ats_Demo.Features.Employee.Queries.GetById;

namespace Ats_Demo.Validators
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
