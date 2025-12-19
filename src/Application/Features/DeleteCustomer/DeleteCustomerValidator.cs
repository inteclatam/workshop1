using FluentValidation;

namespace Intec.Workshop1.Customers.Application.Features.DeleteCustomer;

public class DeleteCustomerValidator : AbstractValidator<DeleteCustomerCommand>
{
    public DeleteCustomerValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Customer ID must be greater than 0");
    }
}
