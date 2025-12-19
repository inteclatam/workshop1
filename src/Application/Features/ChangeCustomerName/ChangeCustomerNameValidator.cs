using FluentValidation;

namespace Intec.Workshop1.Customers.Application.Features.ChangeCustomerName;

public class ChangeCustomerNameValidator : AbstractValidator<ChangeCustomerNameCommand>
{
    public ChangeCustomerNameValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Customer ID must be greater than 0");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required")
            .MaximumLength(100)
            .WithMessage("First name must not exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required")
            .MaximumLength(100)
            .WithMessage("Last name must not exceed 100 characters");
    }
}
