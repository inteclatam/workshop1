namespace Intec.Workshop1.Customers.Application.Features.ChangeCustomerName;

public record ChangeCustomerNameResponse(
    long Id,
    string FullName,
    string FirstName,
    string LastName);
