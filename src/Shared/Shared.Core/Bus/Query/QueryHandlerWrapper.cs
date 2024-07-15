using System;
using System.Threading.Tasks;

namespace Shared.Core.Bus.Query;
public abstract class QueryHandlerWrapper<TResponse>
{
  public abstract Task<TResponse> Handle(IQuery query, IServiceProvider provider);
}

public class QueryHandlerWrapper<TQuery, TResponse> : QueryHandlerWrapper<TResponse>
    where TQuery : IQuery
{
  public override async Task<TResponse> Handle(IQuery query, IServiceProvider provider)
  {
    var handler = provider.GetService(typeof(IQueryHandler<TQuery, TResponse>)) as IQueryHandler<TQuery, TResponse>;
    if (handler == null)
    {
      throw new InvalidOperationException($"Handler for {typeof(TQuery).Name} not found");
    }

    return await handler.Handle((TQuery)query);
  }
}
