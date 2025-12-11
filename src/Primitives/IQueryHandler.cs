namespace Intec.Workshop1.Customers.Primitives;

public interface IQueryHandler<in TQuery, TResponse> 
    where TQuery : IQuery<TResponse>
{
    Task<TResponse> HandleAsync(TQuery query, CancellationToken ct = default);
}