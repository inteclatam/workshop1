using FluentValidation;

namespace Intec.Workshop1.Customers.Application.Features.CreateCustomer;

public class CreateCustomerValidator:AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerValidator()
    {
        RuleFor(c => c.FirstName).NotEmpty().WithMessage("First name is required");
        RuleFor(c => c.LastName).NotEmpty().WithMessage("Last name is required");
        RuleFor(c => c.PhoneNumber).NotEmpty().WithMessage("Phone number is required");
        RuleFor(c => c.EMail).NotEmpty().WithMessage("E-mail is required");
    }
}