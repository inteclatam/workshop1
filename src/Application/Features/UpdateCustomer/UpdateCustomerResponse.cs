namespace Intec.Workshop1.Customers.Application.Features.UpdateCustomer;

public record UpdateCustomerResponse(
    long Id,
    string FullName,
    string Email,
    string PhoneNumber);
