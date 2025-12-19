using Intec.Workshop1.Customers.Infrastructure.Filters;
using Intec.Workshop1.Customers.Primitives;

namespace Intec.Workshop1.Customers.Application.Features.GetAllCustomers;

public static class GetAllCustomersEndpoint
{
    public static IEndpointRouteBuilder MapGetAllCustomersEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/customers", async (
                int? page,
                int? pageSize,
                QueryDispatcher dispatcher,
                CancellationToken cancellationToken) =>
            {
                // Set defaults
                var currentPage = page ?? 1;
                var currentPageSize = pageSize ?? 10;
                currentPage = currentPage <= 0 ? 1 : currentPage;
                currentPageSize = currentPageSize <= 0 ? 10 : currentPageSize;
                currentPageSize = currentPageSize > 100 ? 100 : currentPageSize; // Max 100 items per page

                var query = new GetAllCustomersQuery(currentPage, currentPageSize);
                var result = await dispatcher.DispatchAsync<GetAllCustomersResponse>(query, cancellationToken);

                return Results.Ok(result);
            })
            .AddEndpointFilter<ValidationFilter>()
            .WithName("GetAllCustomers")
            .WithTags("Customers")
            .Produces<GetAllCustomersResponse>(StatusCodes.Status200OK);

        return endpoints;
    }
}
