namespace Intec.Workshop1.Customers.Application.Features.DeleteCustomer;

public record DeleteCustomerResponse(
    long Id,
    bool IsDeleted,
    DateTime? DeletedAt);
