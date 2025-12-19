using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Application.Features.GetAllCustomers;

public record CustomerDto(
    long Id,
    string FullName,
    string Email,
    string PhoneNumber,
    DateTime Created,
    bool IsDeleted);

public record GetAllCustomersResponse(
    IEnumerable<CustomerDto> Items,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages,
    bool HasPreviousPage,
    bool HasNextPage);
