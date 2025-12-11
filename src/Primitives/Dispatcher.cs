namespace Intec.Workshop1.Customers.Primitives;
public class CommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public CommandDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> DispatchAsync<TResponse>(
        ICommand<TResponse> command, 
        CancellationToken ct = default)
    {
        var handlerType = typeof(ICommandHandler<,>)
            .MakeGenericType(command.GetType(), typeof(TResponse));
        
        var handler = _serviceProvider.GetRequiredService(handlerType);
        
        var method = handlerType.GetMethod("HandleAsync")!;
        var result = method.Invoke(handler, new object[] { command, ct });
        
        return await (Task<TResponse>)result!;
    }
}
public class QueryDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public QueryDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> DispatchAsync<TResponse>(
        IQuery<TResponse> query, 
        CancellationToken ct = default)
    {
        var handlerType = typeof(IQueryHandler<,>)
            .MakeGenericType(query.GetType(), typeof(TResponse));
        
        var handler = _serviceProvider.GetRequiredService(handlerType);
        
        var method = handlerType.GetMethod("HandleAsync")!;
        var result = method.Invoke(handler, new object[] { query, ct });
        
        return await (Task<TResponse>)result!;
    }
}